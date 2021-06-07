using GACDDL;
using GACDModels;
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

        public User AddUser(User u)
        {
            Regex emailRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([azA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            bool emailCheck = !emailRegex.IsMatch(u.Email);
            Regex nameRegex = new Regex(@"^[a-zA-Z]{2,}\s[a-zA-Z]{1,}$");
            bool nameCheck = !nameRegex.IsMatch(u.Name);
            Regex usernameRegex = new Regex(@"[a-zA-Z0-9]{3,20}");
            bool usernameCheck = !usernameRegex.IsMatch(u.UserName);
            if (emailCheck || nameCheck || usernameCheck)
            {
                return null;
            }
            else return _repo.AddUser(u);
            
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
