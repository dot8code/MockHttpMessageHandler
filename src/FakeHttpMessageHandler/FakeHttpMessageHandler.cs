using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace dot8code.Tests.FakeHttpMessageHandler
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="resultHttpStatusCode"></param>
        public FakeHttpMessageHandler(T result, HttpStatusCode resultHttpStatusCode)
        {
            _result = result;
            _resultHttpStatusCode = resultHttpStatusCode;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.Generic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public FakeHttpMessageHandler(Exception exception)
        {
            _exception = exception;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.ThrowException;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="httpStatusCode"></param>
        public FakeHttpMessageHandler(HttpContent httpContent, HttpStatusCode httpStatusCode)
        {
            _httpContent = httpContent;
            _resultHttpStatusCode = httpStatusCode;
            _fakeHttpMessageHandlerResultType = FakeHttpMessageHandlerResultType.PassedHttpContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _fakeHttpMessageHandlerResultType switch
            {
                FakeHttpMessageHandlerResultType.Generic => Task.FromResult(MockSend()),
                FakeHttpMessageHandlerResultType.ThrowException => throw _exception,
                FakeHttpMessageHandlerResultType.PassedHttpContent => Task.FromResult<HttpResponseMessage>(
                    MockSendWithHttpContent()),
                _ => throw new NotSupportedException()
            };
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
