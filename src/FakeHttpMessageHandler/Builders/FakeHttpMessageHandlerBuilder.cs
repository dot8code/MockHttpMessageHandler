using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace dot8code.Tests.FakeHttpMessageHandler.Builders
{
    public class FakeHttpMessageHandlerBuilder : IFakeHttpMessageHandlerBuilder
    {
        private HttpContent _content;
        private HttpStatusCode _statusCode;
        private Exception _exceptionToThrow;

        public IFakeHttpMessageHandlerBuilder SetJsonResponse<TIn>(TIn input)
        {
            _content = new StringContent(input != null ? JsonSerializer.Serialize(input) : string.Empty);

            return this;
        }

        public IFakeHttpMessageHandlerBuilder SetStreamResponse(Stream stream)
        {
            _content = new StreamContent(stream);

            return this;
        }

        public IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(int statusCode)
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
            {
                throw new ArgumentException($"Status code: {statusCode} is not defined in {nameof(HttpStatusCode)} enum.");
            }
            
            _statusCode = (HttpStatusCode)statusCode;

            return this;
        }

        public IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;

            return this;
        }

        public IFakeHttpMessageHandlerBuilder SetExceptionToThrow<TException>() where TException : Exception, new()
        {
            _exceptionToThrow = new TException();

            return this;
        }

        public MockHttpMessageHandler<object> Build()
        {
            if (_exceptionToThrow != null)
            {
                return new MockHttpMessageHandler<object>(_exceptionToThrow);
            }
            
            return new MockHttpMessageHandler<object>(_content, _statusCode);
        }

        public HttpClient BuildHttpClient(string baseUrl = "http://baseaddress")
        {
            MockHttpMessageHandler<object> mockHttpMessageHandler;

            if (_exceptionToThrow != null)
            {
                mockHttpMessageHandler = new MockHttpMessageHandler<object>(_exceptionToThrow);
            }
            else
            {
                mockHttpMessageHandler = new MockHttpMessageHandler<object>(_content, _statusCode);
            }

            var httpClient = new HttpClient(mockHttpMessageHandler);
            httpClient.BaseAddress = new Uri(baseUrl);
            
            return httpClient;
        }
    }
}