using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Todo.Api.Interfaces;

/// <summary>
///     Interface for the repository pattern.
///     This interface is used to define the methods that will be implemented in the repository classes.
///     The repository pattern is used to separate the logic that retrieves the data from the database from the business logic.
/// </summary>
/// <typeparam name="TEntity">
///     This entity will be the model that will be used to interact with the database.
/// </typeparam>
/// <typeparam name="TAddDto">
///     This DTO will be used to validate the data that will be added to the database.
/// </typeparam>
/// <typeparam name="TUpdateDto">
///     This DTO will be used to validate the data that will be updated in the database.
/// </typeparam>
public interface IRepository<TEntity, in TAddDto, in TUpdateDto>
    where TEntity : class
    where TAddDto : class
    where TUpdateDto : class
{
    /// <summary>
    ///     This method will return all the entities from the database.
    /// </summary>
    /// <param name="id">
    ///     This parameter will be used to filter the entities by a specific id.
    /// </param>
    /// <returns>
    ///     A Task that will return an IEnumerable of TEntity.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync(string id);

    /// <summary>
    ///     This method will return a specific entity from the database based on the id.
    /// </summary>
    /// <param name="id">
    ///     This parameter will be used to filter the entity by a specific id.
    /// </param>
    /// <returns>
    ///     A Task that will return a TEntity.
    /// </returns>
    Task<TEntity> GetByIdAsync(Guid id);

    /// <summary>
    ///     This method will add a new entity to the database.
    /// </summary>
    /// <param name="entity">
    ///     This parameter will be used to add a new entity to the database.
    /// </param>
    /// <returns>
    ///     A Task that will return the added TEntity.
    /// </returns>
    Task<TEntity> AddAsync(TAddDto entity);

    /// <summary>
    ///     This method will update an entity in the database.
    /// </summary>
    /// <param name="entity">
    ///     This parameter will be used to update an entity in the database.
    /// </param>
    /// <returns>
    ///     A Task that will return the updated TEntity.
    /// </returns>
    Task<TEntity> UpdateAsync(TUpdateDto entity);

    /// <summary>
    ///     This method will update an entity with the data from the DTO.
    /// </summary>
    /// <param name="entity">
    ///     This parameter will be used to update an entity in the database.
    /// </param>
    /// <param name="dto">
    ///     This parameter will be used to update the entity with the data from the DTO.
    /// </param>
    /// <returns>
    ///     The updated TEntity.
    /// </returns>
    TEntity UpdateEntity(TEntity entity, TUpdateDto dto);

    /// <summary>
    ///     This method will delete an entity from the database.
    ///     The entity will be deleted based on the id.
    /// </summary>
    /// <param name="id">
    ///     This parameter will be used to delete an entity from the database.
    /// </param>
    /// <returns>
    ///     A Task that will return nothing.
    /// </returns>
    Task DeleteAsync(Guid id);
}