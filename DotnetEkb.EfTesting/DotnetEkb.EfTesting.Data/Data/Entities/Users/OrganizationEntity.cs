
using System.ComponentModel.DataAnnotations;
using DotnetEkb.EfTesting.CommonData.Interfaces;

namespace DotnetEkb.EfTesting.Data.Entities.Users
{
    public class OrganizationEntity: IWithId
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [MaxLength(15)]
        public string Inn { get; set; }
    }
}