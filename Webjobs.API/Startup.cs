using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;

namespace Webjobs.API
{
	public class Startup
	{
		//public IConfigurationRoot Configuration { get; }

		//public Startup(IConfigurationRoot _configuration)
		//{
		//	Configuration = _configuration;
		//}

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

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseHangfireServer();
			app.UseHangfireDashboard();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Run(async (context) =>
			{
				await context.Response.WriteAsync("Hello World!");
			});
		}
	}
}
