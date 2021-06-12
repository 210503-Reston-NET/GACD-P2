using System;
using Xunit;
using GACDModels;
using GACDBL;
using GACDDL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Serilog;
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
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                int userCount = (await userBL.GetUsers()).Count;
                int expected = 1;
                Assert.Equal(expected, userCount);
            }
        }
        /// <summary>
        /// Makes sure that Categories can be added
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task AddCatShouldAddCatAsync()
        {
            using(var context = new GACDDBContext(options))
            {
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                int catCount = (await categoryBL.GetAllCategories()).Count;
                int expected = 3;
                Assert.Equal(expected, catCount);
            }
        }
        /// <summary>
        /// Makes sure UserStats updates and doesn't fail
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task UserStatShouldAddUserStatAsync()
        {
            using(var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                User user = new User();
                user.Auth0Id = "testid";
                user = await userBL.AddUser(user);
                ICategoryBL categoryBL = new CategoryBL(context);
                Category category = new Category();
                category.Name = 1;
                category = await categoryBL.AddCategory(category);
                IUserStatBL userStatBL = new UserStatBL(context);
                TypeTest typeTest = new TypeTest();
                typeTest.Date = DateTime.Now;
                typeTest.NumberOfErrors = 1;
                typeTest.NumberOfWords = 3;
                typeTest.WPM = 30;
                typeTest.TimeTaken = 5;
                UserStat ust = await userStatBL.AddTestUpdateStat(1, 1, typeTest);
                Assert.NotNull(ust);
            }
        }
        /// <summary>
        /// Makes sure that there aren't any critical failures in Leaderboard / Stat methods
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task OverallLeaderBoardShouldReturnAnyNumberofUsers()
        {
            using (var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(2, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 2, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(3, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 3, testToBeInserted);
                bool expected = true;
                bool actual = (await userStatBL.GetOverallBestUsers()).Count > 0;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure there aren't any critical failures in Category Leaderboard / Stat methods
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task CategoryLeaderBoardShouldReturnAnyNumberofUsers()
        {
            using (var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);

                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(2, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 1, testToBeInserted1);
                TypeTest testToBeInserted2 = await userStatBL.SaveTypeTest(3, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 1, testToBeInserted2);
                bool expected = true;
                bool actual = (await userStatBL.GetBestUsersForCategory(1)).Count > 0;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure all added users are returned correctly in overall leaderboard
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task OverallLeaderBoardShouldReturnCorrectNumberofUsers()
        {
            using(var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                Category category1 = new Category();
                category1.Name = 2;
                await categoryBL.AddCategory(category1);
                Category category2 = new Category();
                category2.Name = 3;
                await categoryBL.AddCategory(category2);
                User user= new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(2, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 2, testToBeInserted);
                testToBeInserted = await userStatBL.SaveTypeTest(3, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 3, testToBeInserted);
                int expected = 3;
                int actual = (await userStatBL.GetOverallBestUsers()).Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure all users are returned correctly for a given category
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task CategoryLeaderBoardShouldReturnCorrectNumberofUsers()
        {
            using (var context = new GACDDBContext(options))
            {
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                
                User user = new User();
                user.Auth0Id = "test";
                await userBL.AddUser(user);
                User user1 = new User();
                user1.Auth0Id = "test1";
                await userBL.AddUser(user1);
                User user2 = new User();
                user2.Auth0Id = "test2";
                await userBL.AddUser(user2);
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(2, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(2, 1, testToBeInserted1);
                TypeTest testToBeInserted2 = await userStatBL.SaveTypeTest(3, 50, 100, DateTime.Now);
                await userStatBL.AddTestUpdateStat(3, 1, testToBeInserted2);
                int expected = 3;
                int actual = (await userStatBL.GetBestUsersForCategory(1)).Count;
                Assert.Equal(expected, actual);
            }
        }
        /// <summary>
        /// Makes sure Average WPM is actual average
        /// </summary>
        /// <returns>True if successful/False on fail</returns>
        [Fact]
        public async Task AverageWPMShouldBeAverage()
        {
            using (var context = new GACDDBContext(options))
            {
                User user = new User();
                user.Auth0Id = "test";
                IUserBL userBL = new UserBL(context);
                ICategoryBL categoryBL = new CategoryBL(context);
                IUserStatBL userStatBL = new UserStatBL(context);
                Category category = new Category();
                category.Name = 1;
                await categoryBL.AddCategory(category);
                await userBL.AddUser(user);
                Double avgExpected;
                TypeTest testToBeInserted = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                avgExpected = testToBeInserted.WPM/2;
                await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted);
                TypeTest testToBeInserted1 = await userStatBL.SaveTypeTest(1, 50, 100, DateTime.Now);
                avgExpected += testToBeInserted1.WPM/2;
                Double actual = (await userStatBL.AddTestUpdateStat(1, 1, testToBeInserted)).AverageWPM;
                Assert.Equal(avgExpected, actual);
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
