﻿using System.Linq.Expressions;
using TumorHospital.WebAPI.DTOs.Pagination;
using TumorHospital.WebAPI.Specifications.Interface;

namespace TumorHospital.WebAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Retriving
        Task<TEntity?> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsNoTrackedAsync();
        IQueryable<TEntity> GetAllAsIQueryable();


        Task<TResult?> GetAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] Includes
            ) where TResult : class;

        Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes
            ) where TResult : class;

        Task<PageSourcePagination<TResult>> GetAllPaginatedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1, int pageSize = 10,
            Expression<Func<TEntity, bool>>? filter = null,
            bool expandable = false,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] Includes)
            where TResult : class;

        Task<List<object>> GetBySpecificationAsync(ISpecification<TEntity> specification);

        #endregion

        #region Add_Update_Delete
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        void Delete(string id);
        Task BulkDeleteAsync(Expression<Func<TEntity, bool>> filter);
        #endregion

        #region Count
        Task<int> Count();
        Task<int> Count(Expression<Func<TEntity, bool>> filter);
        #endregion

        #region Existance
        Task<bool> IsExistAsync(int id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter);
        #endregion

        Task<int> SaveAsync();

    }
}
