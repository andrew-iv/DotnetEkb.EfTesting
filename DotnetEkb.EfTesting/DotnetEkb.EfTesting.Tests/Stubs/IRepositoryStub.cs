using DotnetEkb.EfTesting.Tests.Stubs.Relations;

namespace DotnetEkb.EfTesting.Tests.Stubs
{
    public interface IRepositoryStub
    {
        EntityDependenciesContainer DependenciesContainer { get; set; }
    }
}