using FluentValidation;
using MediatR;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.FoodModule.Contracts;
using PizzaPlace.Infrastructure.Modules.BusinessModule;

namespace PizzaPlace.Application.FoodModule.Commands;

public class UpdateFoodCommand : IRequest
{
    public Guid FoodId { get; set; }
    public string NewName { get; set; }

    public static implicit operator Food(UpdateFoodCommand updateFoodCommand) => new() { Id = updateFoodCommand.FoodId, Name = updateFoodCommand.NewName };
}

public class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
{
    public UpdateFoodCommandValidator()
    {
        RuleFor(food => food.FoodId)
            .NotNull();

        RuleFor(food => food.NewName)
            .NotEmpty();
    }
}

public class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand>
{
    private readonly IFoodRepository _foodRepository;

    public UpdateFoodCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var foodExists = await _foodRepository.RetrieveByIdAsync(request.FoodId);

        if (foodExists != null)
        {
            await _foodRepository.UpdateAsync(request);
        }
        else
        {
            throw new Exception(nameof(ErrorMessages.FoodNotFound));
        }
    }
}