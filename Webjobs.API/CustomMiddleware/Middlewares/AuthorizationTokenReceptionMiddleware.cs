using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Webjobs.API.CustomMiddleware.Middlewares
{
	public class AuthorizationTokenReceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthorizationTokenReceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Business Logic
			await context.Response.WriteAsync("<p>Obteniendo de los headers de la peticion el JWT de Autorizacion para asignarlo a la identidad de usuario...</p>");

			await _next(context);
		}
	}
}
