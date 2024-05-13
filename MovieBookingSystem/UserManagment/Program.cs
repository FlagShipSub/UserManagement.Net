using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserManagment.Configurations;
using UserManagment.Data;
using UserManagment.Models;
using UserManagment.Service;
using UserManagment.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter Bearer Token Ex:- Bearer Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    // Assuming you've imported necessary namespaces like Microsoft.AspNetCore.Authentication.JwtBearer and Swashbuckle.AspNetCore.SwaggerGen

    // Add the security requirement for JWT Bearer authentication scheme
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },
        new string[] {}
    }
});

});
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Myconn")));

var section = builder.Configuration.GetSection("JwtConfig");
var secret = section.GetValue<string>("Secret");
var issuer = section.GetValue<string>("Issuer");
var audience = section.GetValue<string>("Audience");
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x=>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               // services.AddAuthentication("Bearer")
               .AddJwtBearer(opt => {
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

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("default");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();
