using System.Linq;
using DotnetEkb.EfTesting.Data.Entities.Users;
using DotnetEkb.EfTesting.Dto;
using DotnetEkb.EfTesting.Logic;
using DotnetEkb.EfTesting.Tests.Stubs;
using DotnetEkb.EfTesting.Tests.Stubs.Relations;
using FluentAssertions;
using NUnit.Framework;

namespace DotnetEkb.EfTesting.Tests.Logic
{
    [TestFixture]
    public class CommonMockTest
    {
        private RepositoryManagerStub _repoManager;

        [SetUp]
        public void Setup()
        {
            _repoManager = new RepositoryManagerStub();
            _repoManager.DependenciesContainer = new EntityDependenciesContainer(() => _repoManager);
            //TODO: Закоментировать для ДЕМО
            _repoManager.DependenciesContainer.AddOneToManyLink<UserEntity,OrganizationEntity>(x=>x.OrganizationId,x=>x.Organization);

            var firstOrg = new OrganizationEntity()
            {
                Id = 1,
                Name = "NM"
            };
            _repoManager.SetDbEntities(firstOrg,
                new OrganizationEntity()
                {
                    Id = 2,
                    Name = "NM2",
                    Inn = "Inn"
                }
            );
            _repoManager.SetDbEntities(new UserEntity()
            {
                Id = 3,
                OrganizationId = 1,
                LastName = "LastName"
            });
        }

        [Test]
        public void Load_Existing()
        {
            var service = new UsersService(_repoManager);
            service.LoadUser(3).ShouldBeEquivalentTo(new UserInfo
            {
                Fio = "LastName ",
                Id = 3,
                IsActive = true,
                OrganizationName = "NM"
            });
        }

        [Test]
        public void Save_User()
        {
            var service = new UsersService(_repoManager);
            service.SaveUser(new UserInfo()
            {
                Fio = "LastName FN",
                OrganizationInn = "Inn",
                OrganizationName = "OldOrg"
            });
            //TODO: Fail =(
            //var org = _repoManager.GetCommonRepository<OrganizationEntity>().FindById(2);
            var org = _repoManager.GetCommonRepository<OrganizationEntity>().Find(x=>x.Id == 2).LastOrDefault();
            org.ShouldBeEquivalentTo(new OrganizationEntity() {Id = 2, Inn = "Inn", Name = "OldOrg"});
        }

        [Test]
        public void Load_Null()
        {
            var service = new UsersService(_repoManager);
            service.LoadUser(2).ShouldBeEquivalentTo(null);
        }
    }
}