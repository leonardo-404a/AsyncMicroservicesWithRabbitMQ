using FluentValidation;
using MediatR;
using PizzaPlace.Domain.CustomerModule;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.FoodModule.Contracts;
using PizzaPlace.Domain.OrderModule;
using PizzaPlace.Domain.OrderModule.Enums;
using PizzaPlace.Domain.PubSubModule;
using PizzaPlace.Infrastructure.Modules.OrderModule;

namespace PizzaPlace.Application.OrderModule.Commands;

public class CreateOrderCommand : IRequest
{
    public CreateOrderCommand(IEnumerable<Guid> foodIds, OrderType orderType, string? customerName, string? observations, string? address)
    {
        FoodIds = foodIds;
        CustomerName = customerName;
        OrderType = orderType;
        Observations = observations;
        Address = address;

        new CreateOrderCommandValidator().ValidateAndThrow(this);
    }

    public IEnumerable<Guid> FoodIds { get; set; }
    public string? CustomerName { get; set; }
    public OrderType OrderType { get; set; }
    public string? Observations { get; set; }
    public string? Address { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(order => order.FoodIds)
            .NotEmpty();

        RuleFor(order => order.OrderType)
            .NotNull()
            .IsInEnum();
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IBusPublisher _busPublisher;
    private readonly IFoodRepository _foodRepository;
    public CreateOrderCommandHandler(IBusPublisher busPublisher, IFoodRepository foodRepository)
    {
        _busPublisher = busPublisher;
        _foodRepository = foodRepository;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await CreateOrderAsync(request);
        _busPublisher.Publish(new OrderCreatedEvent { Order = order });
    }

    private async Task<Order> CreateOrderAsync(CreateOrderCommand createOrderCommand)
    {
        var order = new Order()
        {
            Id = Guid.NewGuid(),
            Observations = createOrderCommand.Observations,
            OrderStatus = OrderStatus.Pending,
            OrderType = createOrderCommand.OrderType,
            Customer = createOrderCommand.CustomerName != null ? new Customer() { Name = createOrderCommand.CustomerName } : null,
            Address = createOrderCommand.Address,
            Bag = await GetFoodsByIdAsync(createOrderCommand.FoodIds)
        };

        return order;
    }

    private async Task<List<Food>> GetFoodsByIdAsync(IEnumerable<Guid> foodIds)
    {
        var bag = new List<Food>();

        foreach (var foodId in foodIds)
        {
            var food = await _foodRepository.RetrieveByIdAsync(foodId);

            if (food == null)
                throw new Exception("FoodNotFound"); // TODO: Passar para BusinessException

            bag.Add(food);
        }

        return bag;
    }
}
