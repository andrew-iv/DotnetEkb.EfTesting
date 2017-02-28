using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace DotnetEkb.EfTesting.CommonData.Repositories.Interfaces
{
    public interface ICommonRepository<TDbEntity> : ICommonReadonlyRepository<TDbEntity> where TDbEntity : class
    {
        void Add(TDbEntity dbEntity);
        void Add(IEnumerable<TDbEntity> dbEntities);
        void Delete(TDbEntity dbEntity);
        void Delete(IEnumerable<TDbEntity> dbEntities);
        TDbEntity Create();
        void AddOrUpdate(TDbEntity dbEntity, bool @new = false);
        DbEntityEntry<TDbEntity> GetEntry(TDbEntity dbEntity);
        void Reload(TDbEntity dbEntity);
        void Reload(IEnumerable<TDbEntity> dbEntities);
    }
}