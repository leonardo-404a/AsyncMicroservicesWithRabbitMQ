namespace PizzaPlace.Domain.BaseModule.Contracts;

public interface IRepositoryBase<TEntity> where TEntity : Entity
{
    Task InsertAsync(TEntity order);
    Task UpdateAsync(TEntity order);
    Task<TEntity> RetrieveByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> RetrieveAllAsync();
}
