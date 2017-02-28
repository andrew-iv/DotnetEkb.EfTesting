using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DotnetEkb.EfTesting.CommonData.Repositories.Interfaces
{
    public interface  IRepositoryManager
    {
        ICommonRepository<TDbEntity> GetCommonRepository<TDbEntity>() where TDbEntity : class; 
        void SaveChanges();

        IQueryable<TDbRelatedEntity> CollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector) where TDbEntity : class where TDbRelatedEntity : class;
        ICommonReadonlyRepository<TDbEntity> GetCommonReadonlyRepository<TDbEntity>() where TDbEntity : class;

        DbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        IQueryable<TDbRelatedEntity> DbCollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector) where TDbEntity : class where TDbRelatedEntity : class;
    }
}
