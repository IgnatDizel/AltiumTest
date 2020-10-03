using AltiumTest.Data.Abstractions;
using AltiumTest.Data.EF.SQLServer;
using AltiumTest.Middlewares;
using AltiumTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AltiumTest
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSignalR();
      services.AddMvc();
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      services.AddTransient<IMessageService, MessageService>();
      services.AddDbContext<ApplicationContext>(options =>
      options.UseSqlServer(
          _configuration.GetConnectionString("Default"),
          b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddSerilog();
      app.UseMiddleware<SerilogMiddleware>();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<ChatHub>("/chat");
        endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
