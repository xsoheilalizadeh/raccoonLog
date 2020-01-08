using System;
using Microsoft.Extensions.DependencyInjection;

namespace raccoonLog.Http.Stores
{
    public static class FileStoreExtensions
    {
        public static void AddFileStore(this HttpLoggingBuilder builder) => builder.AddFileStore(_ => { });
        public static void AddFileStore(this HttpLoggingBuilder builder, Action<FileStoreOptions> configureOptions)
        {
            var services = builder.Services;

            services.Configure(configureOptions);
            
            
            builder.AddStore<FileStore>();
        }
    }
}