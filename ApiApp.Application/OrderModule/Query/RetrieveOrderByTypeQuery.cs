using FluentValidation;
using MediatR;
using PizzaPlace.Domain.OrderModule;
using PizzaPlace.Domain.OrderModule.Contracts;
using PizzaPlace.Domain.OrderModule.Enums;

namespace PizzaPlace.Application.OrderModule.Query;

public class RetrieveOrderByTypeQuery : IRequest<IEnumerable<Order>>
{
    public RetrieveOrderByTypeQuery(OrderType orderType)
    {
        OrderType = orderType;

        new RetrieveOrderByTypeQueryValidator().ValidateAndThrow(this);
    }

    public OrderType OrderType { get; set; }
}

public class RetrieveOrderByTypeQueryValidator : AbstractValidator<RetrieveOrderByTypeQuery>
{
    public RetrieveOrderByTypeQueryValidator()
    {
        RuleFor(query => query.OrderType)
            .IsInEnum();
    }
}

public class RetrieveOrderByTypeQueryHandler : IRequestHandler<RetrieveOrderByTypeQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public RetrieveOrderByTypeQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> Handle(RetrieveOrderByTypeQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepository.RetrieveByTypeAsync(request.OrderType);
    }
}
