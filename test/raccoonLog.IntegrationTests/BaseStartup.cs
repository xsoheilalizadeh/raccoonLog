using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using raccoonLog.IntegrationTests.Domain;

namespace raccoonLog.IntegrationTests
{
    public class BaseStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpLogging();

            app.UseRouting();

            app.UseEndpoints(endPoints =>
            {
                endPoints.MapGet(EndPoints.GetJson, async context =>
                {
                    var personAsString = JsonSerializer.Serialize(Person.Default);

                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    await context.Response.WriteAsync(personAsString);
                });

                endPoints.MapPost(EndPoints.PostJson, async context =>
                {
                    var person = await JsonSerializer.DeserializeAsync<Person>(context.Request.Body);

                    var personAsString = JsonSerializer.Serialize(person);

                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    await context.Response.WriteAsync(personAsString);
                });

                endPoints.MapGet(EndPoints.GetPlain,
                    async context => { await context.Response.WriteAsync("This is a plain response!"); });

                endPoints.MapPost(EndPoints.PostPlain,
                    async context => { await context.Response.WriteAsync("The response is from a plain request!"); });
            });
        }
    }
}