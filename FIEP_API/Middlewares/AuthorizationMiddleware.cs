using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FIEP_API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().Contains("login"))
            {
                return _next(httpContext);
            }
            var tokenString = httpContext.Request.Headers["token"];
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jsonToken = handler.ReadToken(tokenString);
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = 511;
                return httpContext.Response.WriteAsync("Invalid token");
            }
            var tokenS = handler.ReadToken(tokenString) as JwtSecurityToken;
            var validToTime = tokenS.ValidTo;
            int isExpired = DateTime.Compare(DateTime.UtcNow, validToTime);
            //check expired
            if (isExpired > 0)
            {
                httpContext.Response.StatusCode = 511;
                return httpContext.Response.WriteAsync("Token expired");
            }
            //role
            var roleId = int.Parse(tokenS.Claims.First(x => x.Type == "RoleId").Value);
            var httpMethod = httpContext.Request.Method;
            if(roleId == 1)
            {
                return _next(httpContext);
            }
            switch (httpMethod)
            {
                case "GET":
                    return _next(httpContext);
                case "PUT":
                    if(roleId == 2)
                    {
                        httpContext.Response.StatusCode = 403;
                        return httpContext.Response.WriteAsync("Access denied");
                    }
                    break;
                case "POST":
                    if (roleId == 2)
                    {
                        httpContext.Response.StatusCode = 403;
                        return httpContext.Response.WriteAsync("Access denied");
                    }
                    break;
                case "DELETE":
                    if (roleId == 2)
                    {
                        httpContext.Response.StatusCode = 403;
                        return httpContext.Response.WriteAsync("Access denied");
                    }
                    break;
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
