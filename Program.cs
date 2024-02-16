using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Supabase;
using Supabase.Interfaces;
using System.Text;
using tokenAuth.Context;
using tokenAuth.Helper;
using tokenAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settingsProvider = builder.Services.AddOptions().Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")).BuildServiceProvider();
var settings = settingsProvider.GetRequiredService<IOptions<AppSettings>>().Value;

var url = settings.Supabase_Url;
var key = settings.Supabase_Key;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = settings.Issuer,
        ValidAudience = settings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
    }
);

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true,
    AutoRefreshToken = true
};

builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));

builder.Services.AddScoped<ILoginService, LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
