using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bandwidth.Net.Tests
{
    public sealed class HttpServer: IDisposable
    {
        private readonly RequestHandler _handler;
        private readonly HttpListener _listener;

        public HttpServer(RequestHandler handler, string prefix = null)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handler = handler;
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix ?? "http://localhost:3001/");
            _listener.Start();
            StartHandleRequest();
        }
        public void Dispose()
        {
            ((IDisposable)_listener).Dispose();
        }

        private void StartHandleRequest()
        {
            _listener.GetContextAsync().ContinueWith(HandlerRequest);
        }

        private async void HandlerRequest(Task<HttpListenerContext> obj)
        {
            var context = obj.Result;
            try
            {
                if (!_listener.IsListening) return;
                var request = context.Request;
                var response = context.Response;
                if (_handler.EstimatedMethod != null)
                {
                    Assert.AreEqual(_handler.EstimatedMethod, request.HttpMethod);
                }
                if (_handler.EstimatedPathAndQuery != null)
                {
                    Assert.AreEqual(_handler.EstimatedPathAndQuery, request.Url.PathAndQuery);
                }
                if (_handler.EstimatedContent != null)
                {
                    using (var reader = new StreamReader(request.InputStream, Encoding.UTF8))
                    {
                        Assert.AreEqual(_handler.EstimatedContent, reader.ReadToEnd());    
                    }
                }
                if (_handler.EstimatedHeaders != null)
                {
                    foreach (var estimatedHeader in _handler.EstimatedHeaders)
                    {
                        Assert.AreEqual(estimatedHeader.Value, request.Headers[estimatedHeader.Key]);
                    }
                }
                if (_handler.HeadersToSend != null)
                {
                    foreach (var header in _handler.HeadersToSend)
                    {
                        response.AddHeader(header.Key, header.Value);
                    }
                }
                if (_handler.ContentToSend != null)
                {
                    foreach (var header in _handler.ContentToSend.Headers)
                    {
                        foreach (var val in header.Value)
                        {
                            response.AddHeader(header.Key, val);
                        }
                    }
                    await _handler.ContentToSend.CopyToAsync(response.OutputStream);
                }
                response.StatusCode = _handler.StatusCodeToSend;
                response.Close();
            }
            catch(Exception ex)
            {
                context.Response.Close();
                Error = ex;
            }
        }

        public Exception Error { get; private set; }
    }

    public class RequestHandler
    {
        public RequestHandler()
        {
            StatusCodeToSend = 200;
            EstimatedMethod = "GET";
        }
        public string EstimatedMethod { get; set; }
        public string EstimatedPathAndQuery { get; set; }
        public string EstimatedContent { get; set; }
        public Dictionary<string, string> EstimatedHeaders { get; set; }

        public Dictionary<string, string> HeadersToSend { get; set; }
        public HttpContent ContentToSend { get; set; }
        public int StatusCodeToSend { get; set; }
    }
}
