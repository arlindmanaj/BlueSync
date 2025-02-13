using BlueSync.Data;
using BlueSync.Services.Implementations;
using BlueSync.Services.Interfaces;
using BlueSync.Repositories.Implementations;
using BlueSync.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
               .EnableSensitiveDataLogging());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddSingleton<IWebSocketsService, WebSocketsService>();
builder.Services.AddSingleton<IAudioQueueManager, AudioQueueManager>();

builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IAudioSessionService, AudioSessionService>();
builder.Services.AddScoped<IDeviceGroupService, DeviceGroupService>();

builder.Services.AddScoped<IAudioSessionRepository, AudioSessionRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceGroupRepository, DeviceGroupRepository>();
var app = builder.Build();

app.UseWebSockets();
app.Map("/ws", async (HttpContext context, WebSocketsService webSocketService) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        await webSocketService.AddClient(socket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
