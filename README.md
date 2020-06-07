# MicroMultiTenant Library

MicroMultiTenant is an opinionated micro library targeted currently at ASP.NET Core to easily provide tenant configuration value switching with almost zero config. 

Supported and tested against ASP.NET Core 3.1, 3.0

## Why
* Many frameworks cater for different scenarios, therefore making their API Complex and tedious to setup
* If using `Entity Framework`, libraries often require changes to a `DbContext` involving different interface declarations and constructors, problematic in established applications
* Some frameworks allow for bad practices such as I/O operations during resolution, given resolution generally happens across each http request this can cause significant performance issues if implemented incorrectly by the end user.

## Before you start

The library promotes use of the `IConfiguration` interface as a mechanism for delivering settings, since we can easily plug a variety of inputs which are build during `Startup`:

* Azure Key Vault
* App Settings
* Environment Variables

Currently the library only injects values back into the `HttpContext`, which are then used later in the ASP.NET Core Pipeline. There's intentionally no logic such as handling a non existent tenant since this would require lots of boilerplate code.

## How

First, install the NuGet package:

`Install-Package BobNudd.MicroMultiTenant`

Or 

`dotnet package BobNudd.MicroMultiTenant`

Next implement the interface `IMultiTenant` interface against your tenant resolver class. This is per your requirements, however below is a basic example of a host name resolution strategy:


    public class MultiTenant : IMultiTenant
    {
        private HttpContext _httpContext;

        public MultiTenant(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Task Execute(IConfiguration config)
        {
            var subDomain = string.Empty;
            var host = _httpContext.Request.Host.Host;

            if (!string.IsNullOrWhiteSpace(host))
            {
                subDomain = host.Split('.')[0];
            }

            // example of setting the correct db connection
            var dbConfig = _httpContext.RequestServices.GetRequiredService<IOptions<DbConfig>>();
            dbConfig.Value.ConnectionString = config[$"{subDomain}-SqlConnection"];

            return Task.CompletedTask;
        }
    }


Cool, now we simply added this service to the container:

    public void ConfigureServices(IServiceCollection services)
    {
         services.AddScoped<IMultiTenant, MultiTenant>();
    }
    


And now to install the middleware, add the following to AppBuilder

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
    {
        // middleware above omitted for brevity
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMultiTenant(); // here
    }


That's it, your new Configuration values will be available throughout your application.