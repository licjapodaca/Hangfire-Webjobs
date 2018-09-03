using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Webjobs.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class TestController : ControllerBase
	{
		private readonly ILogger _logger;

		public TestController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TestController>();
		}

		[HttpGet]
		[Route("GetAllData/{ConError}")]
		public async Task<IActionResult> Get(bool conError)
		{
			try
			{
				return await Task.Run(() =>
				{
					if (conError)
					{
						_logger.LogInformation("Entro al evento de error...");
						throw new Exception("Error manejado...");
					}
					else
					{
						_logger.LogInformation("Obteniendo datos...");
						return Ok(new { Id = 1, Nombre = "Jorge Apodaca" });
					}
				});
			}
			catch(Exception)
			{
				throw;
			}
		}
    }
}