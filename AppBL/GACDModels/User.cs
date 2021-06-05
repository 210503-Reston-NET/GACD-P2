using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    class User
    {
        public User() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public List<UserStatCatJoin> UserStatCatJoins { get; set; }
        public List<CompetitionStat> CompetitionStats { get; set; }
    }
}
