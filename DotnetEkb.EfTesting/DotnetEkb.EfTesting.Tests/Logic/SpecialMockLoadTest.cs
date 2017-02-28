using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Data.Entities.Users;
using DotnetEkb.EfTesting.Logic;
using DotnetEkb.EfTesting.Tests.Stubs;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DotnetEkb.EfTesting.Tests.Logic
{
    public class SpecialMockLoadTest
    {
        private IRepositoryManager _repoManager;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IRepositoryManager>();
            var orgRepo = new CommonRepositoryStub<UserEntity>();
            mock.Setup(x => x.GetCommonRepository<UserEntity>()).Returns(orgRepo);
            _repoManager = mock.Object;
        }
        
        [Test]
        public void Load_Null()
        {
            var service = new UsersService(_repoManager);
            service.LoadUser(2).ShouldBeEquivalentTo(null);
        }
    }
}