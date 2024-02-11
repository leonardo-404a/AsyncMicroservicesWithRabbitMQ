using PizzaPlace.Domain.CustomerModule;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.OrderModule.Enums;

namespace PizzaPlace.Domain.OrderModule;

public class Order : Entity
{
    public Order()
    {
        Bag = Enumerable.Empty<Food>();
    }

    public IEnumerable<Food> Bag { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public OrderType OrderType { get; set; }
    public string? Observations { get; set; }
    public Customer? Customer { get; set; }
    public string? Address { get; set; }
}
