
## raccoonLog
> The library development is in progress.

[![codecov](https://codecov.io/gh/xsoheilalizadeh/raccoonLog/branch/master/graph/badge.svg)](https://codecov.io/gh/xsoheilalizadeh/raccoonLog)
[![Build Status](https://travis-ci.org/xsoheilalizadeh/raccoonLog.svg?branch=master)](https://travis-ci.org/xsoheilalizadeh/raccoonLog)

## What is raccoonLog?
raccoonLog is a logging library that supports HTTP request/response logging in **ASP.NET Core 2.2+**

### Features
- Log request/response in console
- Sensitive data protection in request/response
- Ignoring content types and headers in request/response
- Use `System.Text.Json` as JSON API
- Use `System.IO.Pipelines` as I/O API
- Easy to configure

### Packages

 Package name                              | Version                      
-------------------------------------------|-----------------------------
 `raccoonLog.Http` | [![NuGet](https://img.shields.io/nuget/v/raccoonLog.Http.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/raccoonLog.Http/) 


 ### Quick Getting Started
 Use following startup codes to configure raccoonLog in you ASP.NET Core application

 _Learn more in [documentation][doc]_

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddRaccoonLog(builder =>
    {
        builder.AddHttpLogging();
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

### Contribution
This library is young, as young as me and it needs to represent more abilities as a library therefore I need your help for bugs, features, performance improvement.

_Feel free to open PR/issue._

[doc]:http://google.com


