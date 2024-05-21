
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
var section = builder.Configuration.GetSection("JwtConfig");
var secret = section.GetValue<string>("Secret");
var issuer = section.GetValue<string>("Issuer");
var audience = section.GetValue<string>("Audience");
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               // services.AddAuthentication("Bearer")
               .AddJwtBearer(opt =>
               {
                   opt.IncludeErrorDetails = true;
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = false,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateAudience = false,
                       ValidAudience = audience,

                   };
               });

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseRouting();
await app.UseOcelot();

builder.Services.AddAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/Users1", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();
