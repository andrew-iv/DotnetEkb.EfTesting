using System.ComponentModel.DataAnnotations;
using DotnetEkb.EfTesting.CommonData.Interfaces;

namespace DotnetEkb.EfTesting.Data.Entities.Users
{
    public class UserEntity : IWithId
    {
        [Key]
        public int Id { get; set; }

        public virtual OrganizationEntity Organization { get; set; }

        public int OrganizationId { get; set; }

        [Required	()]
        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}