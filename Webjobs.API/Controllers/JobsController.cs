using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webjobs.API.Services.Contracts;

namespace Webjobs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
		private readonly IJobService _jobService;
		private readonly ILogger _logger;

		public JobsController(IJobService jobService, ILoggerFactory loggerFactory) // Injection of IJobService
		{
			_jobService = jobService ?? throw new ArgumentNullException(nameof(jobService)); // Guard
			_logger = loggerFactory.CreateLogger<JobsController>();
		}

		[HttpPost]
		[Route("ExecuteJob/{importance}")]
		public async Task<IActionResult> ExecuteAndDisconnectFromJobAsync(string importance)
		{
			try
			{
				_logger.LogInformation(5001, "Mensaje enviado desde ExecuteAndDisconnectFromJobAsync en la capa del API...");

				return Ok(await _jobService.ExecuteAndDisconnectFromJobAsync(importance));
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}