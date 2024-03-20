using Infrastructure.DB;
using Infrastructure.DB.Repository;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ItemMessagingConfig>(builder.Configuration.GetSection("AzureSB"));

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<ItemMessaging>();
builder.Services.AddSingleton<ItemRepository>();

var connection = builder.Configuration.GetConnectionString("AzureDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connection), ServiceLifetime.Singleton);

var host = builder.Build();
host.Run();
