using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemory.Model;
using Xunit;

namespace InMemory
{
    
    public class TestFixture
    {
        private ApiContext _context;
        private DbContextOptions<ApiContext> _options;

        public TestFixture()
        {
            var builder = new DbContextOptionsBuilder<ApiContext>();
            builder.UseInMemoryDatabase(DateTime.Now.Ticks.ToString());
            _options = builder.Options;
            _context = new ApiContext(_options);
            _context.Users.Add(new User()
            {
                Id = "Id1",
                FirstName = "FN",
                LastName = "LN"
            });
            _context.Posts.Add(new Post()
            {
                Id = "Id1",
                Content = "Cntnt1",
                UserId = "Id2"
            });
            _context.SaveChanges();
        }

        [Fact]
        public void Test()
        {
            Assert.Equal(1, new ApiContext(_options).Users.Count());
        }

        [Fact]
        public void TestAdd()
        {
            lock (this)
            {
                var ctx = new ApiContext(_options);
                ctx.Users.Add(new User()
                {
                    Id = "Id2",
                    FirstName = "FN",
                    LastName = "LN"
                });
                ctx.SaveChanges();
                Assert.Equal(2, new ApiContext(_options).Users.Count());
            }
        }

        [Fact]
        public void TestAddAgain()
        {
            lock (this)
            {
                var ctx = new ApiContext(_options);
                ctx.Users.Add(new User()
                {
                    Id = "Id3",
                    FirstName = "FN",
                    LastName = "LN"
                });
                ctx.SaveChanges();
                Assert.Equal(2, new ApiContext(_options).Users.Count());
            }
        }
    }
}
