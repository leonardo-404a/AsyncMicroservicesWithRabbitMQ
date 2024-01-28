using Microsoft.Extensions.Configuration;
using SharedLib;

namespace ConsumerApp;

public static class Program
{
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

        var eventBusConfig = new EventBusConfig();
        configuration.GetSection("EventBusConfig").Bind(eventBusConfig);

        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException();

        var eventBus = new EventBus(eventBusConfig);

        var orderCreatedHandler = new OrderCreatedHandler(connectionString);

        OrderCreatedHandler orderCreatedHandlerFactory() => new(connectionString);
        eventBus.Subscribe<OrderCreatedEvent>((Func<OrderCreatedHandler>)orderCreatedHandlerFactory);

        Console.WriteLine("Aguardando eventos. Pressione [Enter] para sair.");
        Console.ReadLine();
    }
}
