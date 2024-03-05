using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace dot8code.Tests.FakeHttpMessageHandler.Builders
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFakeHttpMessageHandlerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="TIn"></typeparam>
        /// <returns></returns>
        IFakeHttpMessageHandlerBuilder SetJsonResponse<TIn>(TIn input);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        IFakeHttpMessageHandlerBuilder SetStreamResponse(Stream stream);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(int statusCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        IFakeHttpMessageHandlerBuilder SetStatusCodeResponse(HttpStatusCode statusCode);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        IFakeHttpMessageHandlerBuilder SetExceptionToThrow<TException>() where TException : Exception, new();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        FakeHttpMessageHandler<object> Build();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        HttpClient BuildHttpClient(string baseUrl = "http://baseaddress");
    }
}