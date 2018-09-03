using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Webjobs.API.CustomMiddleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Webjobs.API.Services.Contracts;
using Webjobs.API.Services.Implementations;
using Webjobs.API.Repositories.Contracts;
using Webjobs.API.Repositories.Implementations;
using System;
using Webjobs.API.Tools;

namespace Webjobs.API
{
	public class Startup
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _config;
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger _logger;

		public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
		{
			_config = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();

			_env = env;
			_loggerFactory = loggerFactory;
			_logger = _loggerFactory.CreateLogger<Startup>();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			#region Initial configuration of Hangfire Jobs

			services.AddHangfire(configuration =>
			{
				configuration.UseSqlServerStorage(_config.GetConnectionString("DefaultConnection"));
			});

			#endregion

			#region Set the Dependency Injection (IoC - Iversion of Control)

			services.AddSingleton(_loggerFactory);
			services.AddSingleton<IJobService, JobService>();
			services.AddSingleton<IJobRepository, JobRepository>();
			services.AddSingleton<IHttpCallForJobs, HttpCallForJobs>();

			#endregion

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			SettingLoggers();

			#region Setting the Middleware of Hangfire Jobs

			_logger.LogInformation(5001, $"Number of Workers for Hangfire Jobs: {Environment.ProcessorCount * 5}");

			app.UseHangfireServer(new BackgroundJobServerOptions
			{
				WorkerCount = Environment.ProcessorCount * 5,
				Queues = new string[] { "high_importance", "normal_importance", "low_importance" }
			});

			app.UseHangfireDashboard();

			#endregion

			#region Setting the Exception management

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGlobalExceptionMiddleware(_loggerFactory);

			#endregion

			#region Custom Middleware

			// Asignacion de JWT a la identidad de usuario
			app.UseAuthorizationTokenReceptionMiddleware(_loggerFactory);

			#endregion

			#region Setting the MVC resources

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{id?}");
			});

			#endregion
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
