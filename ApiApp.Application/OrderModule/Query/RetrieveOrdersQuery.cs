using MediatR;
using PizzaPlace.Domain.OrderModule;
using PizzaPlace.Domain.OrderModule.Contracts;

namespace PizzaPlace.Application.OrderModule.Query;

public class RetrieveOrdersQuery : IRequest<IEnumerable<Order>>;

public class RetrieveOrdersQueryHandler : IRequestHandler<RetrieveOrdersQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public RetrieveOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> Handle(RetrieveOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepository.RetrieveAllAsync();
    }
}
