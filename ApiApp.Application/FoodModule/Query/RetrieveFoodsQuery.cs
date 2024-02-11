using MediatR;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.FoodModule.Contracts;

namespace PizzaPlace.Application.FoodModule.Query;

public class RetrieveFoodsQuery : IRequest<IEnumerable<Food>>;

public class RetrieveFoodsQueryHandler : IRequestHandler<RetrieveFoodsQuery, IEnumerable<Food>>
{
    private readonly IFoodRepository _foodRepository;

    public RetrieveFoodsQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<IEnumerable<Food>> Handle(RetrieveFoodsQuery request, CancellationToken cancellationToken)
    {
        return await _foodRepository.RetrieveAllAsync();
    }
}
