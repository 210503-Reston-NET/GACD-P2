using GACDDL;
using GACDModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GACDBL
{
    public class UserBL : IUserBL
    {
        private Repo _repo;
        public UserBL(GACDDBContext context)
        {
            _repo = new Repo(context);
        }

        public async Task<User> AddUser(User u)
        {
            Regex nameRegex = new Regex(@"^[a-zA-Z]{2,}\s[a-zA-Z]{1,}$");
            bool nameCheck = !nameRegex.IsMatch(u.Name);
            Log.Debug(nameCheck.ToString());
            Regex usernameRegex = new Regex(@"[a-zA-Z0-9]{3,20}");
            bool usernameCheck = !usernameRegex.IsMatch(u.UserName);
            Log.Debug(usernameCheck.ToString());
            if (usernameCheck || nameCheck)
            {
                return null;
            }
            return await _repo.AddUser(u);
            
        }

        public async Task<User> GetUser(int id)
        {
            return await _repo.GetUser(id);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _repo.GetAllUsers();
        }
    }
}
