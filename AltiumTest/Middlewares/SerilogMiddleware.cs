using Microsoft.AspNetCore.Http;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AltiumTest.Middlewares
{
  public class SerilogMiddleware
  {
    const string MessageTemplate =
        "{Protocol} {RequestMethod} {Scheme}://{Host}{RequestPath}{QueryString} responded {StatusCode} in {Elapsed:0.0000} ms";

    static readonly Serilog.ILogger _log = Serilog.Log.ForContext<SerilogMiddleware>();

    readonly RequestDelegate _next;

    public SerilogMiddleware(RequestDelegate next)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext httpContext)
    {
      if (httpContext == null) 
        throw new ArgumentNullException(nameof(httpContext));

      var sw = Stopwatch.StartNew();

      try
      {
        await _next(httpContext);
        Log(httpContext, sw);
      }
      catch (Exception ex) when (LogException(httpContext, sw, ex)) 
      { }
    }

    static void Log(HttpContext httpContext, Stopwatch sw)
    {
      sw.Stop();

      int? statusCode = httpContext.Response?.StatusCode;
      LogEventLevel level = statusCode > 499 ? LogEventLevel.Error : statusCode > 399 ? LogEventLevel.Warning : LogEventLevel.Information;

      _log.Write(
        level, 
        MessageTemplate, 
        httpContext.Request.Protocol, 
        httpContext.Request.Method, 
        httpContext.Request.Scheme, 
        httpContext.Request.Host, 
        httpContext.Request.Path,
        httpContext.Request.QueryString, statusCode, sw.Elapsed.TotalMilliseconds);   
    }

    static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
    {
      sw.Stop();

      LogForErrorContext(httpContext)
          .Error(ex, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds);

      return false;
    }

    static Serilog.ILogger LogForErrorContext(HttpContext httpContext)
    {
      var request = httpContext.Request;

      var result = _log
          .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
          .ForContext("RequestHost", request.Host)
          .ForContext("RequestProtocol", request.Protocol);

      if (request.HasFormContentType)
        result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

      return result;
    }
  }
}
