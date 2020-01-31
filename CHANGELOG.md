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