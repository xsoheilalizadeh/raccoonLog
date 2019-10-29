
## raccoonLog
> The library development is in prgress.

[![codecov](https://codecov.io/gh/xsoheilalizadeh/raccoonLog/branch/master/graph/badge.svg)](https://codecov.io/gh/xsoheilalizadeh/raccoonLog)
[![Build Status](https://travis-ci.org/xsoheilalizadeh/raccoonLog.svg?branch=master)](https://travis-ci.org/xsoheilalizadeh/raccoonLog)

### Packages

 Package name                              | Stable                      
-------------------------------------------|-----------------------------
 `raccoonLog.Http` | [![NuGet](https://img.shields.io/nuget/v/raccoonLog.Http.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/raccoonLog.Http/) 


 ### Getting Started

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddRaccoonLog(builder =>
    {
        builder.AddHttpLogging(options =>
        {
            options.EnableConsoleLogging = true;
            options.JsonSerializerOptions.WriteIndented = true;
        });
    });
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseRaccoonLog(builder =>
    {
        builder.EnableHttpLogging();
    });
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
}

```


