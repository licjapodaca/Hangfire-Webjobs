using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Webjobs.API.Models;

namespace Webjobs.API.CustomMiddleware.Middlewares
{
	public class GlobalExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<GlobalExceptionHandlerMiddleware>();
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				// Logic to perform on request

				await _next(context);

				// Logic to perform on response
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error <==================== {ex}");
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(GetCompleteError(context, exception).ToString());
		}

		private static ErrorDetails GetCompleteError(HttpContext context, Exception exception)
		{
			return new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = exception.Message,
				StackTrace = exception.StackTrace,
				InnerException = exception.InnerException != null ? GetCompleteError(context, exception.InnerException) : null
			};
		}
	}
}
