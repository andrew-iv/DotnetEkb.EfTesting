#define EF6
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;

namespace DotnetEkb.EfTesting.CommonData.Repositories
{
    public class CommonRepository<TDbEntity> : CommonReadonlyRepository<TDbEntity>, ICommonRepository<TDbEntity> where TDbEntity : class
    {
        public CommonRepository(DbContext context):base(context)
        {
        }

        public void Add(TDbEntity dbEntity)
        {
            EntitiesSet.Add(dbEntity);
        }

        public void AddOrUpdate(TDbEntity dbEntity, bool @new = false )
        {
            if(@new)
                EntitiesSet.Add(dbEntity);
            else
                Context.Entry(dbEntity).State = EntityState.Modified;
        }

        public void Add(IEnumerable<TDbEntity> dbEntities)
        {
            EntitiesSet.AddRange(dbEntities);
        }

        public void Delete(TDbEntity dbEntity)
        {
            EntitiesSet.Remove(dbEntity);
        }

        public void Delete(IEnumerable<TDbEntity> dbEntities)
        {
            EntitiesSet.RemoveRange(dbEntities);
        }

        public TDbEntity Create()
        {
#if EF6
            return EntitiesSet.Create();
#else
            throw new NotImplementedException();
#endif
        }

        public DbEntityEntry<TDbEntity> GetEntry(TDbEntity dbEntity)
        {
            return Context.Entry(dbEntity);
        }

        public void Reload(TDbEntity dbEntity)
        {
            var entry = GetEntry(dbEntity);
            if (entry.State == EntityState.Added)
            {
                Delete(dbEntity);
            }
            else
            {
                entry.Reload();
            }
        }

        public void Reload(IEnumerable<TDbEntity> dbEntities)
        {
            foreach (var dbEntity in dbEntities)
            {
                Reload(dbEntity);
            }
        }
    }
}
