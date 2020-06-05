
## raccoonLog 🦝 

_development is in progress_

[![codecov](https://codecov.io/gh/xsoheilalizadeh/raccoonLog/branch/master/graph/badge.svg)](https://codecov.io/gh/xsoheilalizadeh/raccoonLog)
[![Build Status](https://travis-ci.org/xsoheilalizadeh/raccoonLog.svg?branch=master)](https://travis-ci.org/xsoheilalizadeh/raccoonLog)

## What is raccoonLog?
It's a HTTP logging library that represents request and response logging for ASP.NET Core applications. 

### Features
- HTTP Request and Response Logging
- Custom form content requests logging  
- Data protection (body limitation)
- Use `System.Text.Json` as JSON API
- Easy to configure
- File data store
- ElasticSearch data store
- Custom data store implementation

### Packages

 Package name                              | Version                      
-------------------------------------------|-----------------------------
 `raccoonLog` | [![NuGet](https://img.shields.io/nuget/v/raccoonLog.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/raccoonLog/) 
 `raccoonLog.Stores.File` | [![NuGet](https://img.shields.io/nuget/v/raccoonLog.Stores.File.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/raccoonLog.Stores.File/) 
 `raccoonLog.Stores.ElasticSearch` | [![NuGet](https://img.shields.io/nuget/v/raccoonLog.Stores.ElasticSearch.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/raccoonLog.Stores.ElasticSearch/) 

 ### Quick Start
 
 Use following startup codes to configure raccoonLog in you ASP.NET Core application

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpLogging().AddFileStore();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
   app.UseHttpLogging();
}
```

### Documentation (soon)

### Contribution
This library is young, as young as me and it needs to represent more abilities as a library therefore I need your help for bugs, features, performance improvement.

_Feel free to open PR/issue._

[doc]:https://github.com/xsoheilalizadeh/raccoonLog/wiki
[1]:https://soheilalizadeh.com/http-logging-in-asp-net-core/

