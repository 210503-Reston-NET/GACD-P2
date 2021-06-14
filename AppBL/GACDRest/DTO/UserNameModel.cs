using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class UserNameModel
    {
        public UserNameModel () { }
        public UserNameModel (string userName, string name)
        {
            this.UserName = userName;
            this.Name = name;
        }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
