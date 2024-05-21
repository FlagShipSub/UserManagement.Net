
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();
app.UseRouting();
await app.UseOcelot();

app.UseEndpoints(endpoints => {
    endpoints.MapGet("/hello", async context => {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();
