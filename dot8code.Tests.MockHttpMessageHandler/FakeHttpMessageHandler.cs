using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace dot8code.Tests.MockHttpMessageHandler
{
    /// <summary>
    /// MessageHandlerTests mock library
    /// </summary>
    /// <typeparam name="T">T is type which will be result of http call.</typeparam>
    public class FakeHttpMessageHandler<T> : HttpMessageHandler
    {
        private readonly T _result;
        private readonly HttpStatusCode _resultHttpStatusCode;

        public FakeHttpMessageHandler(T result, HttpStatusCode resultHttpStatusCode)
        {
            _result = result;
            _resultHttpStatusCode = resultHttpStatusCode;
        }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(MockSend());
        }

        private HttpResponseMessage MockSend()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(_result != null ? JsonSerializer.Serialize(_result) : string.Empty),
                StatusCode = _resultHttpStatusCode
            };
        }
    }
}
