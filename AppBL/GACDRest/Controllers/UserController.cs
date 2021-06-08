using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GACDModels;
using GACDBL;
using Serilog;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GACDRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserBL _userBL;
        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// Posting a new user to the db
        /// TODO:think of a better way
        /// </summary>
        /// <param name="userName">user name to be added</param>
        /// <param name="email">email to be added</param>
        /// <param name="name">Name to be added</param>
        /// <returns></returns>
        // POST api/<UserController>
        [HttpPost]
        //[Route("CreateUser/{userName}/{email}/{name}")]
        public async Task<ActionResult> Post(AddUserModel user)
        {
           
            User u = new User();
            u.Email = user.Email;
            u.UserName = user.UserName;
            u.Name = user.Name;
            bool AddUserFlag = (_userBL.AddUser(u)==null);
            if (!AddUserFlag) return Ok();
            else return BadRequest();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
