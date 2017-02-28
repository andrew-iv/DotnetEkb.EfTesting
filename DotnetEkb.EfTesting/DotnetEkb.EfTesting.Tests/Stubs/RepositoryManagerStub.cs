using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Tests.Helpers.EnumerableHelpers;
using DotnetEkb.EfTesting.Tests.Stubs.Relations;

namespace DotnetEkb.EfTesting.Tests.Stubs
{
    public class RepositoryManagerStub : IRepositoryManager
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public EntityDependenciesContainer DependenciesContainer { get; set; }
        

        public ICommonRepository<TDbEntity> GetCommonRepository<TDbEntity>() where TDbEntity : class
        {
            return GetRepository<ICommonRepository<TDbEntity>, CommonRepositoryStub<TDbEntity>>();
        }

        public ICommonReadonlyRepository<TDbEntity> GetCommonReadonlyRepository<TDbEntity>() where TDbEntity : class
        {
            return GetRepository<ICommonReadonlyRepository<TDbEntity>, CommonRepositoryStub<TDbEntity>>();
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            throw new NotImplementedException();
        }

        public RepositoryManagerStub SetRepository<TDbEntity>(ICommonReadonlyRepository<TDbEntity> repository) where TDbEntity : class
        {
            _cache[typeof(ICommonRepository<TDbEntity>)] = repository;
            _cache[typeof(ICommonReadonlyRepository<TDbEntity>)] = repository;
            return this;
        }

        public RepositoryManagerStub SetDbEntities<TDbEntity>(params TDbEntity[] entities) where TDbEntity : class
        {
            return SetDbEntities(entities as IEnumerable<TDbEntity>);
        }

        public RepositoryManagerStub SetDbEntities<TDbEntity>(IEnumerable<TDbEntity> entities) where TDbEntity : class
        {
            return SetRepository<TDbEntity>(new CommonRepositoryStub<TDbEntity>(entities, DependenciesContainer));
        }

        public virtual void SaveChanges()
        {
            SaveChangesCounter++;
        }

        public int SaveChangesCounter { get; private set; }

        public IQueryable<TDbRelatedEntity> CollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector)
            where TDbEntity : class where TDbRelatedEntity : class
        {
            return selector.Compile().Invoke(entity).AsQueryable();
        }

        public IQueryable<TDbRelatedEntity> DbCollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector) where TDbEntity : class where TDbRelatedEntity : class
        {
            throw new NotImplementedException();
        }

        private TNewResult GetRepository<TInterface,TNewResult>() where TNewResult : IRepositoryStub, new()
        {
            var type = typeof(TInterface);
            var result = (TNewResult)_cache.GetValue(type);
            if (result != null)
            {
                return result;
            }
            result = new TNewResult();
            _cache.Add(type, result);
            result.DependenciesContainer = DependenciesContainer;
            return result;
        }
    }
}