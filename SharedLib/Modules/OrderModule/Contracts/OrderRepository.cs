using MongoDB.Driver;
using PizzaPlace.Domain.OrderModule;
using PizzaPlace.Domain.OrderModule.Contracts;
using PizzaPlace.Domain.OrderModule.Enums;
using PizzaPlace.Infrastructure.Modules.BaseModule.Contracts;

namespace PizzaPlace.Infrastructure.Modules.OrderModule.Contracts;

public class OrderRepository : RepositoryMongoDBBase<Order>, IOrderRepository
{
    private const string COLLECTION_NAME = "Orders";
    public OrderRepository(
        MongoDbConfig mongoDBConfig,
        IMongoClient mongoClient
    ) : base(mongoDBConfig.DatabaseName, mongoClient, COLLECTION_NAME)
    { }

    public async Task<IEnumerable<Order>> RetrieveByTypeAsync(OrderType type)
    {
        return await Collection.Find(order => order.OrderType == type).ToListAsync();
    }

    public async Task<IEnumerable<Order>> RetrieveByStatusAsync(OrderStatus status)
    {
        return await Collection.Find(order => order.OrderStatus == status).ToListAsync();
    }
}
