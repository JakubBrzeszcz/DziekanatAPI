using CoreApp.Common;
using CoreApp.Entities;
using CoreApp.Pagination;
using CoreApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGenericRepository<T> : IGenericRepositoryAsync<T> where T : EntityBase
{
    private readonly DbSet<T> _set;

    public EfGenericRepository(DbSet<T> set)
    {
        _set = set;
    }

    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await _set.ToListAsync();
    }

    public async Task<CoreApp.Pagination.PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Numer strony musi być większy lub równy 1.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Rozmiar strony musi być większy lub równy 1.");

        var totalCount = await _set.CountAsync();
        var items = await _set
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new CoreApp.Pagination.PagedResult<T>(items, totalCount, page, pageSize);
    }

    public async Task<T> AddAsync(T entity)
    {
        var entry = await _set.AddAsync(entity);
        return entry.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (!await _set.AsNoTracking().AnyAsync(e => e.Id == entity.Id))
        {
            throw new KeyNotFoundException($"Encja typu {typeof(T).Name} o ID {entity.Id} nie została znaleziona.");
        }
        var entityEntry = _set.Update(entity);
        return entityEntry.Entity;
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await FindByIdAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Encja typu {typeof(T).Name} o ID {id} nie została znaleziona.");
        
        _set.Remove(entity);
    }
}