namespace SelfResearch.Financial.API.Feature.Propagate;

public interface IPropagatedEntityService<T> where T : class
{

    /// <summary>
    /// Get entity by its Id
    /// </summary>
    /// <param name="id">The unique identifier</param>
    /// <returns>The entity</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Create a new entity
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <returns>The entity</returns>
    Task<T?> CreateAsync(T entity);
}
