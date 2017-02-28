using System;
using System.Linq;
using DotnetEkb.EfTesting.CommonData.Repositories.Interfaces;
using DotnetEkb.EfTesting.Data.Entities.Users;
using DotnetEkb.EfTesting.Dto;

namespace DotnetEkb.EfTesting.Logic
{
    public class UsersService
    {
        private readonly IRepositoryManager _repositoryManager;

        public UsersService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public UserInfo LoadUser(int id)
        {
            var entity = UserRepo.FindById(id);
            if (entity == null)
                return null;
            return new UserInfo()
            {
                Fio = entity.LastName + " " + entity.FirstName,
                IsActive = true,
                OrganizationName = entity.Organization.Name,
                OrganizationInn = entity.Organization.Inn,
                Id = id
            };
        }

        private ICommonRepository<UserEntity> UserRepo => _repositoryManager.GetCommonRepository<UserEntity>();

        public void SaveUser(UserInfo userInfo)
        {
            var entity = UserRepo.FindById(userInfo.Id);
            var fio = userInfo.Fio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool isNew = entity == null;
            entity = entity?? new UserEntity();
            entity.Id = userInfo.Id;
            entity.Organization = SaveOrganization(userInfo);
            entity.LastName = fio.First();
            entity.FirstName = fio.Skip(1).LastOrDefault();

            if (isNew)
                UserRepo.Add(entity);
            _repositoryManager.SaveChanges();
        }

        private OrganizationEntity SaveOrganization(UserInfo userInfo)
        {
            var repository = _repositoryManager.GetCommonRepository<OrganizationEntity>();
            var entity = repository.Find(x=>x.Inn == userInfo.OrganizationInn).FirstOrDefault();
            bool isNew = entity == null;
            entity = entity?? new OrganizationEntity();
            entity.Id = entity?.Id??0;
            entity.Inn = userInfo.OrganizationInn;
            entity.Name = userInfo.OrganizationName;
            if (isNew)
                repository.Add(entity);
            return entity;
        }
    }
}