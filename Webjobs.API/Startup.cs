using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Webjobs.API.CustomMiddleware;
using Microsoft.Extensions.Logging;

namespace Webjobs.API
{
	public class Startup
	{

		public Startup(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHangfire(configuration =>
			{
				configuration.UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=Hangfire-WebJobs;User ID=sa;pwd=bts123!");
			});

			services.AddMvc();
		}

		// A

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			app.UseGlobalExceptionMiddleware();

			app.UseHangfireServer();
			app.UseHangfireDashboard();

			//if (env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}

			#region Custom Middleware

			// Asignacion de JWT a la identidad de usuario
			app.UseAuthorizationTokenReceptionMiddleware();

			//app.Use(async (context, next) =>
			//{
			//	// Logic to perform on request
			//	//await context.Response.WriteAsync("<p>Hello from middleware 1!</p>");
			//	await next();
			//	// Logic to perform on response
			//});

			#endregion

			app.UseMvc();

			// B
		}
	}
}
