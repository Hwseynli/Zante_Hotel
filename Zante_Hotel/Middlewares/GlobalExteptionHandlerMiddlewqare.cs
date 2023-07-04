using System;
using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Middlewares
{
	public class GlobalExteptionHandlerMiddlewqare
	{
        private readonly RequestDelegate _next;

        public GlobalExteptionHandlerMiddlewqare(RequestDelegate next)
		{
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);

            }
        }
        public void HandleException(HttpContext context, Exception exception)
        {
            context.Response.Redirect($"/Home/ErrorPage?errorMessage={exception.Message}");
        }
    }

}

