using Task = System.Threading.Tasks.Task;

namespace API.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(string comparable);

    Task<T?> GetByIdAsync(string id);

    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(string id);
}