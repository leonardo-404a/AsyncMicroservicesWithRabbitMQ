using FluentValidation;
using MediatR;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.FoodModule.Contracts;

namespace PizzaPlace.Application.FoodModule.Commands;

public class CreateFoodCommand : IRequest
{
    public CreateFoodCommand(string name)
    {
        Name = name;

        new CreateFoodCommandValidator().ValidateAndThrow(this);
    }

    public string Name { get; set; }
    public static implicit operator Food(CreateFoodCommand createFoodCommand) => new() { Id = Guid.NewGuid(), Name = createFoodCommand.Name };
}

public class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
{
    public CreateFoodCommandValidator()
    {
        RuleFor(food => food.Name)
            .NotEmpty();
    }
}

public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand>
{
    private readonly IFoodRepository _foodRepository;

    public CreateFoodCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var foodExists = await _foodRepository.RetrieveByName(request.Name);

        if (foodExists == null)
        {
            await _foodRepository.InsertAsync(request);
        }
    }
}
