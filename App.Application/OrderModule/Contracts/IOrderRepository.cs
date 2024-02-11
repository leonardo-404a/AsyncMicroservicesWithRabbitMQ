using PizzaPlace.Domain.BaseModule.Contracts;
using PizzaPlace.Domain.OrderModule.Enums;

namespace PizzaPlace.Domain.OrderModule.Contracts;

public interface IOrderRepository : IRepositoryBase<Order>
{
    Task<IEnumerable<Order>> RetrieveByStatusAsync(OrderStatus status);
    Task<IEnumerable<Order>> RetrieveByTypeAsync(OrderType type);
}
