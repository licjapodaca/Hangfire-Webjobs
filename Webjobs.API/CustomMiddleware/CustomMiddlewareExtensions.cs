using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Webjobs.API.CustomMiddleware.Middlewares;

namespace Webjobs.API.CustomMiddleware
{
	public static class CustomMiddlewareExtensions
	{
		public static IApplicationBuilder UseAuthorizationTokenReceptionMiddleware(this IApplicationBuilder builder, ILoggerFactory loggerFactory)
		{
			return builder.UseMiddleware<AuthorizationTokenReceptionMiddleware>(loggerFactory);
		}

		public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder, ILoggerFactory loggerFactory)
		{
			return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>(loggerFactory);
		}
	}
}
