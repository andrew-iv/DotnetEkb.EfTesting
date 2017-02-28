
using DotnetEkb.EfTesting.CommonData.Interfaces;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;

namespace DotnetEkb.EfTesting.Tests.Stubs.Relations
{
    public interface ILink<TObject> where TObject : class  
    {
        void Link(IRepositoryManager repositoryManager, TObject @object);
    }
}