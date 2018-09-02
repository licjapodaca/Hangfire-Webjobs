using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Webjobs.API.CustomMiddleware.Middlewares
{
	public class AuthorizationTokenReceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public AuthorizationTokenReceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<AuthorizationTokenReceptionMiddleware>();
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Logic to perform on request
			_logger.LogInformation("Iniciando peticion y pasando por AuthorizationTokenReceptionMiddleware...");

			await _next(context);

			// Logic to perform on response
			_logger.LogInformation("Terminando peticion y pasando nuevamente por AuthorizationTokenReceptionMiddleware...");

		}
	}
}
