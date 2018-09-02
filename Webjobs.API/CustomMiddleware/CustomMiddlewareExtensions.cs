using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Webjobs.API.CustomMiddleware.Middlewares;
using Webjobs.API.Models;

namespace Webjobs.API.CustomMiddleware
{
	public static class CustomMiddlewareExtensions
	{
		public static IApplicationBuilder UseAuthorizationTokenReceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<AuthorizationTokenReceptionMiddleware>();
		}

		public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
