using System.Data.Entity;
using DotnetEkb.EfTesting.Data.Entities.Users;

namespace Xrm.NAO.Social.Data
{
    public interface ISocialDbContext
    {
        IDbSet<UserEntity> Users { get; set; }
        IDbSet<OrganizationEntity> Organizations { get; set; }
        int SaveChanges();
    }
}