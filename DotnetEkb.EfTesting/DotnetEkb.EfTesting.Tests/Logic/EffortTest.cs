using DotnetEkb.EfTesting.CommonData.Repositories;
using DotnetEkb.EfTesting.Data.Entities.Users;
using DotnetEkb.EfTesting.Dto;
using DotnetEkb.EfTesting.Logic;
using Effort.Extra;
using FluentAssertions;
using NUnit.Framework;
using Xrm.NAO.Social.Data;

namespace DotnetEkb.EfTesting.Tests.Logic
{
    public class EffortTest
    {
        private EffortContainer<SocialDbContext> _effortContainer;
        private RepositoryManager _repoManager;

        public static void GetReferenceData(ObjectData data)
        {
            data.Table<OrganizationEntity>("OrganizationEntities").Add(new OrganizationEntity()
            {
                Id = 1, Inn="Inn1", Name = "Name1"
            });
            data.Table<OrganizationEntity>("OrganizationEntities").Add(new OrganizationEntity()
            {
                Id = 2,
                Inn = "Inn2",
                Name = "Name2"
            });

            data.Table<UserEntity>("UserEntities").Add(new UserEntity()
            {
                Id = 3,
                OrganizationId = 1,
                LastName = "LastName"
            });
        }

        [SetUp]
        public void Setup()
        {
            _effortContainer = new EffortContainer<SocialDbContext>(x=> new SocialDbContext(x));
            _effortContainer.FillData(GetReferenceData);
            _repoManager = _effortContainer.RepositoryManager;
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
                OrganizationName = "Name1",
                OrganizationInn = "Inn1"
            });
        }

        [Test]
        public void Save_User()
        {
            var service = new UsersService(_repoManager);
            service.SaveUser(new UserInfo()
            {
                Fio = "LastName FN",
                OrganizationInn = "Inn1",
                OrganizationName = "OldOrg"
            });

            var assertManager = _effortContainer.RepositoryManager;
            var org = assertManager.GetCommonRepository<OrganizationEntity>().FindById(1);
            org.ShouldBeEquivalentTo(new OrganizationEntity() { Id = 1, Inn = "Inn1", Name = "OldOrg" });
        }

        [Test]
        public void Save_User_New()
        {
            var service = new UsersService(_repoManager);
            service.SaveUser(new UserInfo()
            {
                Fio = "LastName FN",
                OrganizationInn = "Inn1",
                OrganizationName = "OldOrg"
            });

            var assertManager = _effortContainer.RepositoryManager;
            var org = assertManager.GetCommonRepository<OrganizationEntity>().FindById(1);
            org.ShouldBeEquivalentTo(new OrganizationEntity() { Id = 1, Inn = "Inn1", Name = "OldOrg" });
        }

        [Test]
        public void Load_Null()
        {
            var service = new UsersService(_repoManager);
            service.LoadUser(2).ShouldBeEquivalentTo(null);
        }
    }
}