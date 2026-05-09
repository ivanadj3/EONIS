using DrinkStore.API.Db;
using DrinkStore.API.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // your Vue dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DrinkStoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt["Key"]!)
                )
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("VueApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapProductEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapOrderEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DrinkStoreDbContext>();
    db.Database.Migrate();
}

app.Run();

