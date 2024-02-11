using MongoDB.Driver;
using PizzaPlace.Domain.FoodModule;
using PizzaPlace.Domain.FoodModule.Contracts;
using PizzaPlace.Infrastructure.Modules.BaseModule.Contracts;

namespace PizzaPlace.Infrastructure.Modules.FoodModule.Contracts;

public class FoodRepository : RepositoryMongoDBBase<Food>, IFoodRepository
{
    private const string COLLECTION_NAME = "Foods";
    public FoodRepository(
        MongoDbConfig mongoDBConfig,
        IMongoClient mongoClient
    ) : base(mongoDBConfig.DatabaseName, mongoClient, COLLECTION_NAME)
    { }

    public async Task<Food> RetrieveByName(string name)
    {
        return await Collection.Find(food => food.Name == name).FirstOrDefaultAsync();
    }
}
