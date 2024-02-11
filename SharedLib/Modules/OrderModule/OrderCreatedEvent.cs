using PizzaPlace.Domain.OrderModule;

namespace PizzaPlace.Infrastructure.Modules.OrderModule;

public class OrderCreatedEvent
{
    public Order Order { get; set; }
}