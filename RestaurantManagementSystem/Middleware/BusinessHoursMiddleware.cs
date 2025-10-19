using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Middleware
{
    public class BusinessHoursMiddleware
    {
        private readonly RequestDelegate _next;

        public BusinessHoursMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var now = DateTime.Now;
            var day = now.DayOfWeek;
            var hour = now.Hour;

            // Restaurant is open from Monday to Saturday 10AM-10PM
            if ((day == DayOfWeek.Sunday) || (hour < 10 || hour >= 22))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("The restaurant is currently closed. We're open Monday-Saturday 10AM-10PM.");
                return;
            }

            await _next(context);
        }
    }
}