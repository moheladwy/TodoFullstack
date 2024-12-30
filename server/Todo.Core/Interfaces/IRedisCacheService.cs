namespace Todo.Core.Interfaces;

/// <summary>
///     Interface for Redis Cache Service
/// </summary>
public interface IRedisCacheService
{
    /// <summary>
    ///     Get data from Redis Cache
    /// </summary>
    /// <param name="key">
    ///     Key to get data from Redis Cache
    /// </param>
    /// <typeparam name="T">
    ///     Type of data to get from Redis Cache
    /// </typeparam>
    /// <returns>
    ///     Data from Redis Cache with the given key if exists, otherwise default value of the type
    /// </returns>
    Task<T?> GetData<T>(string key);

    /// <summary>
    ///     Set data in Redis Cache
    /// </summary>
    /// <param name="key">
    ///     Key to set data in Redis Cache
    /// </param>
    /// <param name="data">
    ///     Data to set in Redis Cache
    /// </param>
    /// <typeparam name="T">
    ///     Type of data to set in Redis Cache
    /// </typeparam>
    /// <returns>
    ///     A task that represents the asynchronous operation
    /// </returns>
    Task SetData<T>(string key, T data);

    /// <summary>
    ///     Update data in Redis Cache
    /// </summary>
    /// <param name="key">
    ///     Key to update data in Redis Cache
    /// </param>
    /// <param name="data">
    ///     Data to update in Redis Cache
    /// </param>
    /// <typeparam name="T">
    ///     Type of data to update in Redis Cache
    /// </typeparam>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the updated data
    /// </returns>
    Task<T> UpdateData<T>(string key, T data);

    /// <summary>
    ///     Remove data from Redis Cache
    /// </summary>
    /// <param name="key">
    ///     Key to remove data from Redis Cache
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation
    /// </returns>
    Task RemoveData(string key);
}