using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace dot8code.Tests.FakeHttpMessageHandler.Tests;

[TestFixture]
public class FakeHttpMessageHandlerTests
{
    [Test]
    public async Task SendAsync_Generic_ReturnsExpectedResponse()
    {
        // Arrange
        var expectedContent = new TestObject { Message = "Test" };
        var handler = new FakeHttpMessageHandler<object>(expectedContent, HttpStatusCode.OK);
        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        JsonSerializer.Deserialize<TestObject>(content)?.Message.Should().Be("Test");
    }

    [Test]
    public async Task SendAsync_Exception_ThrowsException()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        var handler = new FakeHttpMessageHandler<object>(expectedException);
        var client = new HttpClient(handler);

        // Act
        Func<Task> action = async () => await client.GetAsync("https://example.com");

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Test exception");
    }

    [Test]
    public async Task SendAsync_HttpContent_ReturnsExpectedResponse()
    {
        // Arrange
        var expectedContent = new StringContent("Test");
        var handler = new FakeHttpMessageHandler<object>(expectedContent, HttpStatusCode.OK);
        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Test");
    }

    [Test]
    public async Task SendAsync_NullContent_ReturnsEmptyContent()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler<object>(null, HttpStatusCode.OK);
        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().BeEmpty();
    }

    [Test]
    public async Task SendAsync_NullResult_ReturnsEmptyContent()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler<object>(null, HttpStatusCode.OK);
        var client = new HttpClient(handler);

        // Act
        var response = await client.GetAsync("https://example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().BeEmpty();
    }
    
    internal class TestObject
    {
        public required string Message { get; init; }
    }
}