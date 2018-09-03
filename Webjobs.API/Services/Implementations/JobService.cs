using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webjobs.API.Repositories.Contracts;
using Webjobs.API.Services.Contracts;
using Webjobs.API.Tools;

namespace Webjobs.API.Services.Implementations
{
	public class JobService : IJobService
	{
		private readonly IJobRepository _jobRepository;
		private readonly ILogger _logger;
		private readonly IHttpCallForJobs _httpCall;

		public JobService(IJobRepository jobRepository, IHttpCallForJobs httpCall, ILoggerFactory loggerFactory) // Injection of IJobService
		{
			_jobRepository = jobRepository ?? throw new ArgumentNullException(nameof(jobRepository)); // Guard
			_logger = loggerFactory.CreateLogger<JobService>();
			_httpCall = httpCall;
		}

		public async Task<string> ExecuteAndDisconnectFromJobAsync(string importance)
		{
			try
			{
				_logger.LogInformation(5001, "Mensaje enviado desde ExecuteAndDisconnectFromJobAsync en la capa de Servicio...");
				
				return await _httpCall.GenerarMensaje(importance);

				//await _jobRepository.ExecuteAndDisconnectFromJobAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
