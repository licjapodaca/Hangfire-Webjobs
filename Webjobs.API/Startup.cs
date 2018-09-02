using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Webjobs.API.CustomMiddleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace Webjobs.API
{
	public class Startup
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _config;
		private readonly ILoggerFactory _loggerFactory;

		public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
		{
			_config = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();

			_env = env;
			_loggerFactory = loggerFactory;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHangfire(configuration =>
			{
				configuration.UseSqlServerStorage(_config.GetConnectionString("DefaultConnection"));
			});

			services.AddSingleton(_loggerFactory);

			services.AddMvc();
		}

		// A

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			SettingLoggers();
			
			app.UseHangfireServer();
			app.UseHangfireDashboard();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGlobalExceptionMiddleware(_loggerFactory);

			#region Custom Middleware

			// Asignacion de JWT a la identidad de usuario
			app.UseAuthorizationTokenReceptionMiddleware(_loggerFactory);

			#endregion

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{id?}");
			});

			// B
		}

		#region Private Methods

		private void SettingLoggers()
		{
			if (_config.GetValue("Loggers:Console:Enabled", false))
			{
				_loggerFactory.AddConsole(_config.GetValue("Loggers:Console:LogLevel", LogLevel.Error));
			}

			if (_config.GetValue("Loggers:Debug:Enabled", false))
			{
				_loggerFactory.AddDebug(_config.GetValue("Loggers:Debug:LogLevel", LogLevel.Error));
			}

			if (_config.GetValue("Loggers:EventLog:Enabled", false))
			{
				_loggerFactory.AddEventLog(GetEventLoggingSettings());
			}
		}

		private EventLogSettings GetEventLoggingSettings()
		{
			return new EventLogSettings()
			{
				LogName = _config.GetValue<string>("Loggers:EventLog:LogName"),
				SourceName = _config.GetValue<string>("Loggers:EventLog:SourceName"),
				Filter = (source, level) => level >= _config.GetValue("Loggers:EventLog:LogLevel", LogLevel.Error)
			};
		}

		#endregion
	}
}
