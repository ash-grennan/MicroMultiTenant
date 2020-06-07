using Microsoft.AspNetCore.Builder;
using System;

namespace BobNudd.MicroMultiTenant
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MultiTenantExtension
    {
        /// <summary>
        /// Adds the Multi Tenant middleware to the specified <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> 
        /// </summary>
        /// <param name="builder">application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenantMiddleware>();
        }
    }
}
