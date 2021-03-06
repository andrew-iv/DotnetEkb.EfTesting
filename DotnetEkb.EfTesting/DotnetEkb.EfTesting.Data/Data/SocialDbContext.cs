using System.Data.Common;
using System.Data.Entity;
using DotnetEkb.EfTesting.Data.Entities.Users;

namespace DotnetEkb.EfTesting.Data
{
    public class SocialDbContext : DbContext, ISocialDbContext
    {
        public SocialDbContext()
            : this("SocialEntities")
        {
        }

        public SocialDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public SocialDbContext(DbConnection existingConnection) : base(existingConnection, true)
        {
        }

        public IDbSet<UserEntity> Users { get; set; }

        public IDbSet<OrganizationEntity> Organizations { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                /*.Some*/;
        }
    }
}