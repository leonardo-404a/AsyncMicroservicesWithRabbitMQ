using MongoDB.Driver;
using PizzaPlace.Domain;
using PizzaPlace.Domain.BaseModule.Contracts;

namespace PizzaPlace.Infrastructure.Modules.BaseModule.Contracts;

public class RepositoryMongoDBBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
{
    private readonly string _database;
    private readonly IMongoClient _mongoClient;
    private readonly string _collection;

    public RepositoryMongoDBBase(string database, IMongoClient mongoClient, string collection)
    {
        (_database, _mongoClient, _collection) = (database, mongoClient, collection);

        if (_mongoClient.GetDatabase(_database).ListCollectionNames().ToList().Contains(collection) is false)
            _mongoClient.GetDatabase(_database).CreateCollection(collection);
    }

    protected virtual IMongoCollection<TEntity> Collection =>
        _mongoClient.GetDatabase(_database).GetCollection<TEntity>(_collection);

    public async Task InsertAsync(TEntity entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Collection.ReplaceOneAsync(dbEntity => dbEntity.Id == entity.Id, entity);
    }

    public async Task<TEntity> RetrieveByIdAsync(Guid id)
    {
        return await Collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> RetrieveAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }
}
