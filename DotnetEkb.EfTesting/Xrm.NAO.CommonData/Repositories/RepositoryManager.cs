#define EF6
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Interfaces;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;

namespace DotnetEkb.EfTesting.CommonData.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly DbContext _context;

        public RepositoryManager(DbContext context)
        {
            _context = context;
        }

        public ICommonRepository<TDbEntity> GetCommonRepository<TDbEntity>() where TDbEntity : class
        {
            return new CommonRepository<TDbEntity>(_context);
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = string.Join(Environment.NewLine+"-----"+ Environment.NewLine, ex.EntityValidationErrors.Select(e => string.Join(Environment.NewLine, e.ValidationErrors.Select(v => string.Format("{0} - {1}", v.PropertyName, v.ErrorMessage)))));
                var error = string.Format("SaveChanges {0}{1}Validation errors:{1}{2}", 
                    ex, Environment.NewLine, errors);
                throw new InvalidOperationException(error, ex);
            }
        }

        public IQueryable<TDbRelatedEntity> CollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector) where TDbEntity : class where TDbRelatedEntity : class
        {
            var entry = _context.Entry(entity);
            var collection = entry.Collection(selector);
            if (entry.State == EntityState.Added)
            {
                return (selector.Compile()(entity) ?? Enumerable.Empty<TDbRelatedEntity>()).AsQueryable();
            }
            if (entry.State == EntityState.Detached || collection.IsLoaded)
                return collection.CurrentValue.AsQueryable();
            return collection.Query();
        }

        public IQueryable<TDbRelatedEntity> DbCollectionQuery<TDbEntity, TDbRelatedEntity>(TDbEntity entity, Expression<Func<TDbEntity, ICollection<TDbRelatedEntity>>> selector) where TDbEntity : class where TDbRelatedEntity : class
        {
            return _context.Entry(entity).Collection(selector).Query();
        }

        public ICommonReadonlyRepository<TDbEntity> GetCommonReadonlyRepository<TDbEntity>() where TDbEntity : class
        {
            return new CommonReadonlyRepository<TDbEntity>(_context);
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return _context.Database.BeginTransaction(isolationLevel);
        }
    }
}