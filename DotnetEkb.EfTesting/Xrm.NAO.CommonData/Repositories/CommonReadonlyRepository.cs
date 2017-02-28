#define EF6
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;

namespace DotnetEkb.EfTesting.CommonData.Repositories
{
    public class CommonReadonlyRepository<TDbEntity>: RepositoryBase<TDbEntity>, ICommonReadonlyRepository<TDbEntity> where TDbEntity : class
    {
        public CommonReadonlyRepository(DbContext context) : base(context)
        {
        }

        public TDbEntity FirstOrDefault(Expression<Func<TDbEntity, bool>> filter = null)
        {
            return Find(filter).FirstOrDefault();
        }

        public TDbEntity First(Expression<Func<TDbEntity, bool>> filter = null)
        {
            return Find(filter).First();
        }

        public TDbEntity FindById(params object[] id)
        {
            return EntitiesSet.Find(id);
        }

        public IQueryable<TDbEntity> Find(Expression<Func<TDbEntity, bool>> filter = null)
        {
            return filter == null ? EntitiesSet : EntitiesSet.Where(filter);
        }
    }
}
