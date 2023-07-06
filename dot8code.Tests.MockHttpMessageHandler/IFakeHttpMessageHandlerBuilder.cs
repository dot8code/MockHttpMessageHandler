using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace dot8code.Tests.MockHttpMessageHandler
{
    public interface IFakeHttpMessageHandlerBuilder
    {
        IFakeHttpMessageHandlerBuilder SetJsonResponse<TIn>(TIn input);
        IFakeHttpMessageHandlerBuilder SetStreamResponse(Stream stream);
        IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(int statusCode);
        IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(HttpStatusCode statusCode);
        IFakeHttpMessageHandlerBuilder SetExceptionToThrow<TException>() where TException : Exception, new();
        FakeHttpMessageHandler<object> Build();
        HttpClient BuildHttpClient(string baseUrl);
    }
}