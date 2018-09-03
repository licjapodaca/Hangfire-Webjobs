using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webjobs.API.Repositories.Contracts;

namespace Webjobs.API.Repositories.Implementations
{
	public class JobRepository : IJobRepository
	{
		private readonly ILogger _logger;

		public JobRepository(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<JobRepository>();
		}

		public async Task ExecuteAndDisconnectFromJobAsync()
		{
			try
			{
				await Task.Run(() =>
				{
					_logger.LogInformation(5001, "Mensaje generado desde ExecuteAndDisconnectFromJobAsync en la capa de Repositorio...");
				});
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
