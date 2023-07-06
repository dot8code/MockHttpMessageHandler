using System;
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
        private readonly Exception _exception;
        private readonly HttpContent _httpContent;
        private readonly FakeHttpMessageHandlerResultType _fakeHttpMessageHandlerResultType;

        public FakeHttpMessageHandler(T result, HttpStatusCode resultHttpStatusCode)
        {
            _result = result;
            _resultHttpStatusCode = resultHttpStatusCode;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.Generic;
        }

        public FakeHttpMessageHandler(Exception exception)
        {
            _exception = exception;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.ThrowException;
        }

        public FakeHttpMessageHandler(HttpContent httpContent, HttpStatusCode httpStatusCode)
        {
            _httpContent = httpContent;
            _resultHttpStatusCode = httpStatusCode;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.PassedHttpContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            switch (_fakeHttpMessageHandlerResultType)
            {
                case FakeHttpMessageHandlerResultType.Generic:
                    return Task.FromResult(MockSend());
                case FakeHttpMessageHandlerResultType.ThrowException:
                    throw _exception;
                case FakeHttpMessageHandlerResultType.PassedHttpContent:
                    return Task.FromResult<HttpResponseMessage>(MockSendWithHttpContent());
                default:
                    throw new NotSupportedException();
            }
        }

        private HttpResponseMessage MockSendWithHttpContent()
        {
            return new HttpResponseMessage
            {
                Content = _httpContent,
                StatusCode = _resultHttpStatusCode
            };
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

    internal enum FakeHttpMessageHandlerResultType
    {
        Generic,
        ThrowException,
        PassedHttpContent
    }
}
