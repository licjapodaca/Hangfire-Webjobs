using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webjobs.API.Filters;

namespace Webjobs.API.Tools
{
	public class HttpCallForJobs : IHttpCallForJobs
	{
		private readonly ILogger _logger;

		public HttpCallForJobs(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<HttpCallForJobs>();
		}

		public async Task<string> GenerarMensaje(string importance)
		{
			try
			{
				return await Task.Run(() =>
				{
					string JobId = String.Empty;

					switch (importance)
					{
						case "high_importance":
							JobId = BackgroundJob.Enqueue(() => GenerarMensajeHigh());
							break;
						case "normal_importance":
							JobId = BackgroundJob.Enqueue(() => GenerarMensajeNormal());
							break;
						case "low_importance":
							JobId = BackgroundJob.Enqueue(() => GenerarMensajeLow());
							break;
					}

					return JobId;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		[Queue("high_importance")]
		//[ProlongExpirationTime]
		public async Task GenerarMensajeHigh()
		{
			try
			{
				await Task.Delay(10000);
				_logger.LogInformation(5001, "Metodo ejecutado como HIGH Job...");
			}
			catch (Exception)
			{
				throw;
			}
		}

		[Queue("normal_importance")]
		//[ProlongExpirationTime]
		public async Task GenerarMensajeNormal()
		{
			try
			{
				await Task.Delay(15000);
				_logger.LogInformation(5001, "Metodo ejecutado como NORMAL Job...");
			}
			catch (Exception)
			{
				throw;
			}
		}

		[Queue("low_importance")]
		//[ProlongExpirationTime]
		public async Task GenerarMensajeLow()
		{
			try
			{
				await Task.Delay(20000);
				_logger.LogInformation(5001, "Metodo ejecutado como LOW Job...");
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
