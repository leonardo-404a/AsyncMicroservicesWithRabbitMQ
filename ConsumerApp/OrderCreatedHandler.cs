using Dapper;
using Microsoft.Data.SqlClient;
using PizzaPlace.Domain.PubSubModule.Event;
using PizzaPlace.Infrastructure.Modules.OrderModule;

namespace PizzaPlace.OrderService;

public class OrderCreatedHandler(string connectionString) : IEventHandler<OrderCreatedEvent>
{
    private readonly string _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    public void Handle(OrderCreatedEvent @event)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var orderEntity = new
            {
                @event.Order.Customer,
                @event.Order.Observations
            };

            var insertSql = "INSERT INTO Orders (Customer, Details) VALUES (@Customer, @Details)";

            connection.Execute(insertSql, orderEntity);
            connection.Close();
        }

        Console.WriteLine($"Evento recebido: Novo pedido criado - ID: {@event.Order.Id}, Cliente: {@event.Order.Customer}");
    }
}
