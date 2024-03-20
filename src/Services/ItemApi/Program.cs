using Microsoft.EntityFrameworkCore;
using Infrastructure.DB;
using Infrastructure.DB.Repository;
using Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ItemMessagingConfig>(builder.Configuration.GetSection("AzureSB"));
builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<ItemMessaging>();

var connection = builder.Configuration.GetConnectionString("AzureDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connection));

var app = builder.Build();

app.Use((context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    }
    return next();
});

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

