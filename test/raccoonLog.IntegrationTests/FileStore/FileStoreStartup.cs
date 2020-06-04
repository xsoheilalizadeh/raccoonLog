using Microsoft.Extensions.DependencyInjection;
using raccoonLog.Stores.File;

namespace raccoonLog.IntegrationTests.FileStore
{
    public class FileStoreStartup : BaseStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            services.AddHttpLogging().AddFileStore();
        }
    }
}