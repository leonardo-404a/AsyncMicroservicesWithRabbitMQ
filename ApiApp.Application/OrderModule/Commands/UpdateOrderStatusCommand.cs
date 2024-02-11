using FluentValidation;
using MediatR;
using PizzaPlace.Domain.OrderModule.Contracts;
using PizzaPlace.Domain.OrderModule.Enums;
using PizzaPlace.Infrastructure.Modules.BusinessModule;

namespace PizzaPlace.Application.OrderModule.Commands;

public class UpdateOrderStatusCommand : IRequest
{
    public UpdateOrderStatusCommand(Guid orderId, OrderStatus newOrderStatus)
    {
        OrderId = orderId;
        NewOrderStatus = newOrderStatus;

        new UpdateOrderStatusCommandValidator().ValidateAndThrow(this);
    }

    public Guid OrderId { get; set; }
    public OrderStatus NewOrderStatus { get; set; }
}

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(order => order.OrderId)
            .NotNull();

        RuleFor(order => order.NewOrderStatus)
            .IsInEnum();
    }
}

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var orderExists = await _orderRepository.RetrieveByIdAsync(request.OrderId);

        if (orderExists == null)
        {
            throw new Exception(nameof(ErrorMessages.OrderNotFound));
        }

        orderExists.OrderStatus = request.NewOrderStatus;
        await _orderRepository.UpdateAsync(orderExists);
    }
}
