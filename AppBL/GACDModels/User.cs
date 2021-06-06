using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<UserStatCatJoin> UserStatCatJoins { get; set; }
        public List<CompetitionStat> CompetitionStats { get; set; }
    }
}
