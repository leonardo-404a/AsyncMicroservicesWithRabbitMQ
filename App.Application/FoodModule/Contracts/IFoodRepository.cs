using PizzaPlace.Domain.BaseModule.Contracts;

namespace PizzaPlace.Domain.FoodModule.Contracts;

public interface IFoodRepository : IRepositoryBase<Food>
{
    Task<Food> RetrieveByName(string name);
}
