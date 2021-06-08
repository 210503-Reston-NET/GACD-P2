using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDModels;
using GACDBL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatController : ControllerBase
    {
        private IUserStatBL _userStatBL;
        public UserStatController(IUserStatBL userstat)

        {
            _userStatBL = userstat;
        }
        // GET: api/<UserStatController>
        /// <summary>
        /// Method for getting all the users stats as a list per category
        /// </summary>
        /// <param name="id">Id of user whose stats you are looking for</param>
        /// <returns>List of user stats for the given user</returns>
        [HttpGet("all/{id}")]
        public async Task<IEnumerable<UserStat>> GetAsync(int id)
        {
            return await _userStatBL.GetUserStats(id);
        }

        // GET api/<UserStatController>/5
        /// <summary>
        /// Method for getting the average user stats accross the board
        /// for a user
        /// </summary>
        /// <param name="id">Id of user whose stats you are looking for</param>
        /// <returns>Average user stats for the given user</returns>
        [HttpGet("{id}")]
        public async Task<UserStat> GetAvgAsync(int id)
        {
            return await _userStatBL.GetAvgUserStat(id);
        }

        // POST api/<UserStatController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserStatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserStatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
