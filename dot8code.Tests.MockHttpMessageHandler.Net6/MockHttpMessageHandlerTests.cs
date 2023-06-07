using System.Net;
using System.Text.Json;
using FluentAssertions;
using HttpClient = System.Net.Http.HttpClient;

namespace dot8code.Tests.MockHttpMessageHandler.Net6;

public class MockHttpMessageHandlerTests
{
    [Fact]
    public async Task Should_CreateMockHttpMessageHandler()
    {
        //Arrange
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        const string expectedResponse = "expected rsponse";

        //Act
        var mockedMessage = new FakeHttpMessageHandler<string>(expectedResponse, expectedStatusCode);
        var httpClient = new HttpClient(mockedMessage);
        httpClient.BaseAddress = new Uri("http://baseaddress");
        var result = await httpClient.GetAsync("");

        //Assert
        result.Should().NotBeNull();
        result.IsSuccessStatusCode.Should().BeTrue();

        var resultContent = await DeserializeHttpResponse<string>(result);
        resultContent.Should().NotBeNull();
        resultContent.Should().Be(expectedResponse);
    }

    private async Task<T?> DeserializeHttpResponse<T>(HttpResponseMessage httpResponseMessage)
    {
        var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
        if (readAsStringAsync is null)
        {
            throw new ArgumentException($"{nameof(httpResponseMessage)} can not be read.");
        }

        return JsonSerializer.Deserialize<T>(readAsStringAsync);
    }
}