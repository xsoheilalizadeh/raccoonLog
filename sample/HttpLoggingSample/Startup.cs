using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using raccoonLog;
using raccoonLog.Stores.ElasticSearch;
using raccoonLog.Stores.File;

namespace HttpLoggingSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // services.AddHttpLogging().AddFileStore();

                services.AddHttpLogging().AddElasticSearchStore(options =>
                {
                    options.Index = "raccoon-elk";
                    options.Url = "http://localhost:9200";
                });
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/Error", async (ctx) =>
                {
                    await ctx.Response.WriteAsync("This a error");

                    ctx.Response.StatusCode = 500;
                });
            });
        }
    }
}
