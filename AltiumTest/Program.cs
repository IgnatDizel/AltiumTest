using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace AltiumTest
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

      try
      {
        Log.Information("Starting web host");
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception exception)
      {
        Log.Error(exception, "Stopped program");
        throw;
      }
      finally
      {
        Log.CloseAndFlush();
      }   
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
