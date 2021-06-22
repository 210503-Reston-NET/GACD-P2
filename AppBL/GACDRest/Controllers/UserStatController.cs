using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDModels;
using GACDBL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Serilog;
using GACDRest.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatController : ControllerBase
    {
        private IUserStatBL _userStatBL;
        private IUserBL _userBL;
        private ICategoryBL _categoryBL;
        public UserStatController(IUserStatBL userstat, IUserBL userBL, ICategoryBL categoryBL)

        {
            _userStatBL = userstat;
            _userBL = userBL;
            _categoryBL = categoryBL;
        }

        /// <summary>
        /// GET /api/UserStat/all
        /// Method for getting all the users stats listed per category
        /// </summary>
        /// <param name="id">Id of user whose stats you are looking for</param>
        /// <returns>List of DTO of user stats for the given user or 404 if none found</returns>
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StatModel>>> GetAsync()
        {
            try
            {
                User u = new User();
                u.Auth0Id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                u = await _userBL.GetUser(u.Auth0Id);
                List<UserStatCatJoin> uscjs =  await _userStatBL.GetUserStats(u.Id);
                List<StatModel> statModels = new List<StatModel>();
                foreach(UserStatCatJoin userStatCatJoin in uscjs)
                {
                    UserStat userStat = await _userStatBL.GetUserStatByUSId(userStatCatJoin.UserStatId);
                    Category category = await _categoryBL.GetCategoryById(userStatCatJoin.CategoryId);
                    statModels.Add(new StatModel(u.Auth0Id, userStat.AverageWPM, userStat.AverageAccuracy, userStat.NumberOfTests, userStat.TotalTestTime, category.Name));
                }
                return statModels;
            }
            catch (Exception)
            {
                Log.Error("Can't get stats");
                return NotFound();
            }
        }

        /// <summary>
        /// GET /api/UserStat/tests
        /// Used to get current user’s latest 100 tests as a list of tests sorted by date (oldest to newest).
        /// </summary>
        /// <returns>List of DTO of user tests for the given user or 404 if user not found</returns>
        [HttpGet("tests")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TestStatOutput>>> GetTests()
        {
            try
            {
                User u = new User();
                u.Auth0Id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                u = await _userBL.GetUser(u.Auth0Id);
                List<TypeTest> typeTests = await _userStatBL.GetTypeTestsForUser(u.Id);
                List<TestStatOutput> typeTestOutputs = new List<TestStatOutput>();
                foreach (TypeTest t in typeTests)
                {
                    TestStatOutput typeTestOutput = new TestStatOutput();
                    typeTestOutput.date = t.Date;
                    typeTestOutput.numberofcharacters = t.NumberOfWords;
                    typeTestOutput.numberoferrors = t.NumberOfErrors;
                    typeTestOutput.timetakenms = t.TimeTaken;
                    typeTestOutput.wpm = t.WPM;
                    typeTestOutputs.Add(typeTestOutput);
                }
                return typeTestOutputs;
            }catch(Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Unable to retrive tests");
                return NotFound();
            }
        }

        /// <summary>
        /// GET /api/UserStat
        /// Used to get user’s average statistics, category returned is set to be user’s revapoints.
        /// </summary>
        /// <returns>DTO for Average user stats for the given user ir 404 if not found</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<StatModel>> GetAvgAsync()
        {
            try
            {
                User u = new User();
                u.Auth0Id = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                u = await _userBL.GetUser(u.Auth0Id);
                UserStat userStat = await _userStatBL.GetAvgUserStat(u.Id);
                return new StatModel(u.Id.ToString(), userStat.AverageWPM, userStat.AverageAccuracy, userStat.NumberOfTests, userStat.TotalTestTime, u.Revapoints);
            }
            catch (Exception)
            {
                Log.Error("Error in getting userstat average");
                return NotFound();
            }
        }
        
    }
}
