#define EF6
using System.Data.Entity;

#if EF6

#else
using Microsoft.Data.Entity;
#endif


namespace DotnetEkb.EfTesting.CommonData.Repositories
{
    public abstract class RepositoryBase<TDbEntity> where TDbEntity : class
    {
        protected RepositoryBase(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; }

        protected DbSet<TDbEntity> EntitiesSet => Context.Set<TDbEntity>();
    }
}
