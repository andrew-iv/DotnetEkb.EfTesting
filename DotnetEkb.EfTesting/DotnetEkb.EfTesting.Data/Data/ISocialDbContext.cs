using System.Data.Entity;
using DotnetEkb.EfTesting.Data.Entities.Users;

namespace DotnetEkb.EfTesting.Data
{
    public interface ISocialDbContext
    {
        IDbSet<UserEntity> Users { get; set; }
        IDbSet<OrganizationEntity> Organizations { get; set; }
        int SaveChanges();
    }
}