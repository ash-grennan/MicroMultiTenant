using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BobNudd.MicroMultiTenant
{
    public class MultiTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IServiceProvider provider)
        {
            var context = provider.GetRequiredService<IMultiTenant>();
            var config = provider.GetRequiredService<IConfiguration>();
            context.Execute(config);

            // push the updated services back into the <see cref="T:Microsoft.AspNetCore.Http.HttpContext" />  HttpContext
            httpContext.RequestServices = httpContext.RequestServices.CreateScope().ServiceProvider;

            return _next(httpContext);
        }
    }
}
