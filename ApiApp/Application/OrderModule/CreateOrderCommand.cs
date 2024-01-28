using SharedLib;

namespace ApiApp.Application.OrderModule;

public class CreateOrderCommand
{
    public string Customer { get; set; }
    public string Details { get; set; }

    public static implicit operator Order(CreateOrderCommand createOrder) => new(createOrder.Customer, createOrder.Details);
}
