
using SharedLib;

namespace ApiApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var eventBusConfig = new EventBusConfig();
        builder.Configuration.GetSection("EventBusConfig").Bind(eventBusConfig);

        var eventBus = new EventBus(eventBusConfig);

        builder.Services.AddSingleton<IBusPublisher>(eventBus);
        builder.Services.AddSingleton<IBusSubscriber>(eventBus);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
