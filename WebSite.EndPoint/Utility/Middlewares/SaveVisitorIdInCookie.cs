using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebSite.EndPoint.Utility.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SaveVisitorIdInCookie
    {
        private readonly RequestDelegate _next;

        public SaveVisitorIdInCookie(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //Request
            string visitordId = httpContext.Request.Cookies["VisitorId"];
            if (visitordId == null)
            {
                visitordId = Guid.NewGuid().ToString();
                httpContext.Response.Cookies.Append("VisitorId", visitordId,
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(30),                        
                    });
            }
            return _next(httpContext);
            //Response
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SaveVisitorIdInCookieExtensions
    {
        public static IApplicationBuilder UseSaveVisitorIdInCookie(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SaveVisitorIdInCookie>();
        }
    }
}
