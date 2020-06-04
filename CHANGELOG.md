
# 2.1.0 (2020-06-4)
This version has a lot of breaking changes, if you had any problems please make a new issue for that.

* ElasticSearch DataStore
* Update System.Text.Json
* Drop .NET Core 2.2 Support
* New Console logging format
* More Configurable logging
* Storing logs in queue for reducing network latency 
* Error Logging

# 2.0.4 (2020-04-16)

### Improvements

* Update Dependencies ([#24](pr-24))

### Bug Fixes

*  Fix JsonException on empty json body ([#23](pr-23))

# 2.0.3 (2020-02-5)

### Bug Fixes

*  Use TryAdd for adding claims to log object

# 2.0.2 (2020-01-31)

### Bug Fixes

* FileStore - Append correct log format when exception occurred ([#15][pr-15])


# 2.0.2 (2020-01-31)

### Bug Fixes

* FileStore - Append correct log format when exception occurred ([#15][pr-15])


# 2.0.1 (2020-01-24)

### Bug Fixes

* Resetting the request body stream position


# 2.0.0 (2020-01-17)

### Features

* Add Custom logging store
* Add Physical file store 

### Improvements

* Add ASP.NET Core 3.1 support
* Replacing `Task` with `ValueTask` in handlers
* Avoid copying `CancellationToken` in logging middleware


### Breaking Changes

* Drop user agent parser support


[pr-15]:https://github.com/xsoheilalizadeh/raccoonLog/pull/15
[pr-23]:https://github.com/xsoheilalizadeh/raccoonLog/pull/23
[pr-24]:https://github.com/xsoheilalizadeh/raccoonLog/pull/24