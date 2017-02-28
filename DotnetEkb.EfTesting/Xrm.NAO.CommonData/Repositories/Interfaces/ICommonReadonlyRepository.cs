using System;
using System.Linq;
using System.Linq.Expressions;

namespace DotnetEkb.EfTesting.CommonData.Repositories.Interfaces
{
    public interface ICommonReadonlyRepository<TDbEntity> 
    {
        TDbEntity FindById(params object[] id);
        IQueryable<TDbEntity> Find(Expression<Func<TDbEntity, bool>> filter = null);
        TDbEntity FirstOrDefault(Expression<Func<TDbEntity, bool>> filter = null);
        TDbEntity First(Expression<Func<TDbEntity, bool>> filter = null);
    }
}