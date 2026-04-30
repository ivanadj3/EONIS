using DrinkStore.API.Endpoints;

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

var app = builder.Build();

app.UseCors("VueApp");

app.MapProductEndpoints();

app.Run();

