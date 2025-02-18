using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ProfileService.Business;
using ProfileService.Repository;
using ProfileService.Services;

Env.Load();
var host = Env.GetString("DB_HOST");
var port = Env.GetInt("DB_PORT");
var username = Env.GetString("DB_USERNAME");
var password = Env.GetString("DB_PASSWORD");
var databaseName = Env.GetString("DB_DATABASE");


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<SomethingService>();
builder.Services.AddScoped<SomethingRepository>();
builder.Services.AddDbContext<DatabaseContext>(opt => { opt
        .UseNpgsql($"Host={host};Port={port};Username={username};Password={password};Database={databaseName}")
        .UseSnakeCaseNamingConvention();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();