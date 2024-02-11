using FluentValidation;
using MediatR;
using PizzaPlace.Domain.OrderModule;
using PizzaPlace.Domain.OrderModule.Contracts;
using PizzaPlace.Domain.OrderModule.Enums;

namespace PizzaPlace.Application.OrderModule.Query;

public class RetrieveOrderByStatusQuery : IRequest<IEnumerable<Order>>
{
    public RetrieveOrderByStatusQuery(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;

        new RetrieveOrderByStatusQueryValidator().ValidateAndThrow(this);
    }

    public OrderStatus OrderStatus { get; set; }
}

public class RetrieveOrderByStatusQueryValidator : AbstractValidator<RetrieveOrderByStatusQuery>
{
    public RetrieveOrderByStatusQueryValidator()
    {
        RuleFor(query => query.OrderStatus)
            .IsInEnum();
    }
}

public class RetrieveOrderByStatusQueryHandler : IRequestHandler<RetrieveOrderByStatusQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public RetrieveOrderByStatusQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> Handle(RetrieveOrderByStatusQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepository.RetrieveByStatusAsync(request.OrderStatus);
    }
}
