using System;
using Xunit;
using GACDModels;
using GACDBL;
using GACDDL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GACDTests
{
    public class GACDUnitTests
    {
        private readonly DbContextOptions<GACDDBContext> options;
        public GACDUnitTests()
        {
            options = new DbContextOptionsBuilder<GACDDBContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }
        /// <summary>
        /// Method to make sure AddUser adds a user to the db
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddUserShouldAddUserAsync()
        {
            using(var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Email = "abc@xyz.com";
                user.Name = "Jack Ryan";
                user.UserName = "abc123";
                await userBL.AddUser(user);
                int userCount = (await userBL.GetUsers()).Count;
                int expected = 1;
                Assert.Equal(expected, userCount);
            }
        }
        [Fact]
        public async Task AddCatShouldAddCatAsync()
        {
            using(var context = new GACDDBContext(options))
            {
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                int catCount = (await categoryBL.GetAllCategories()).Count;
                int expected = 1;
                Assert.Equal(expected, catCount);
            }
        }
        private void Seed()
        {
            using(var context = new GACDDBContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}