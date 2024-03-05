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

        public FakeHttpMessageHandler<object> Build()
        {
            if (_exceptionToThrow != null)
            {
                return new FakeHttpMessageHandler<object>(_exceptionToThrow);
            }
            
            return new FakeHttpMessageHandler<object>(_content, _statusCode);
        }

        public HttpClient BuildHttpClient(string baseUrl = "http://baseaddress")
        {
            FakeHttpMessageHandler<object> fakeHttpMessageHandler;

            if (_exceptionToThrow != null)
            {
                fakeHttpMessageHandler = new FakeHttpMessageHandler<object>(_exceptionToThrow);
            }
            else
            {
                fakeHttpMessageHandler = new FakeHttpMessageHandler<object>(_content, _statusCode);
            }

            var httpClient = new HttpClient(fakeHttpMessageHandler);
            httpClient.BaseAddress = new Uri(baseUrl);
            
            return httpClient;
        }
    }
}