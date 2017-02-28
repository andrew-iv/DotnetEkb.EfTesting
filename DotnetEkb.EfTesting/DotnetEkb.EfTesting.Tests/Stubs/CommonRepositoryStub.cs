using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Tests.Helpers.BaseTypeHelpers;
using DotnetEkb.EfTesting.Tests.Stubs.Relations;

namespace DotnetEkb.EfTesting.Tests.Stubs
{
    public class PrimaryKeyInfo<TDbEntity>
    {
        private readonly Func<TDbEntity, object>[] _accessors;

        public PrimaryKeyInfo()
        {
            _accessors = typeof(TDbEntity).GetProperties()
                .Where(prop => prop.GetCustomAttributes(typeof(KeyAttribute)).Any())
                .Select(x => new Func<TDbEntity, object>(entity => x.GetMethod.Invoke(entity, new object[] { })))
                .ToArray();
        }

        public Func<TDbEntity, bool> IsThisKey(object[] key)
        {
            return
                entity =>
                    _accessors.Zip(key, (accessor, keyPart) => ObjectHelper.EqualsNull(accessor(entity), keyPart)).All(x => x);
        }
    }

    public class CommonRepositoryStub<TDbEntity> : ICommonRepository<TDbEntity>, IRepositoryStub where TDbEntity : class
    {
        private readonly List<TDbEntity> _data = new List<TDbEntity>();
        private static readonly PrimaryKeyInfo<TDbEntity> PkInfo = new PrimaryKeyInfo<TDbEntity>();

        public EntityDependenciesContainer DependenciesContainer { get; set; }

        public CommonRepositoryStub(IEnumerable<TDbEntity> data, EntityDependenciesContainer dependenciesContainer = null)
        {
            DependenciesContainer = dependenciesContainer;
            Add(data);
        }

        public CommonRepositoryStub() : this(Enumerable.Empty<TDbEntity>())
        {
        }

        public void Add(TDbEntity dbEntity)
        {
            DependenciesContainer?.Link(dbEntity);
            _data.Add(dbEntity);
        }

        public void Add(IEnumerable<TDbEntity> dbEntities)
        {
            var entities = dbEntities as TDbEntity[] ?? dbEntities.ToArray();
            _data.AddRange(entities);
            foreach (var dbEntity in entities)
            {
                DependenciesContainer?.Link(dbEntity);
            }
        }

        public TDbEntity FirstOrDefault(Expression<Func<TDbEntity, bool>> filter)
        {
            return Find().FirstOrDefault(filter);
        }

        public TDbEntity First(Expression<Func<TDbEntity, bool>> filter)
        {
            return Find().First(filter);
        }

        public Func<TDbEntity, object> IdExpression { get; set; }

        public TDbEntity FindById(object[] id)
        {
            var predic = PkInfo.IsThisKey(id);
            return Find().FirstOrDefault(predic);
        }

        public IQueryable<TDbEntity> Find(Expression<Func<TDbEntity, bool>> filter = null)
        {
            return filter == null ? _data.AsQueryable() : _data.AsQueryable().Where(filter);
        }

        public void Delete(TDbEntity dbEntity)
        {
            _data.Remove(dbEntity);
        }

        public void Delete(IEnumerable<TDbEntity> dbEntities)
        {
            foreach (var toDel in dbEntities.Distinct())
            {
                Delete(toDel);
            }
        }

        public TDbEntity Create()
        {
            return (TDbEntity)Activator.CreateInstance(typeof(TDbEntity));
        }

        public void AddOrUpdate(TDbEntity dbEntity, bool @new = false)
        {
            Add(dbEntity);
        }

        public DbEntityEntry<TDbEntity> GetEntry(TDbEntity dbEntity)
        {
            throw new NotImplementedException();
        }

        public void Reload(TDbEntity dbEntity)
        {
            throw new NotImplementedException();
        }

        public void Reload(IEnumerable<TDbEntity> dbEntities)
        {
            throw new NotImplementedException();
        }

        DbEntityEntry<TDbEntity> ICommonRepository<TDbEntity>.GetEntry(TDbEntity dbEntity)
        {
            throw new NotImplementedException();
        }
    }
}