using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class UserStat
    {
        public UserStat() { }
        public int Id{ get; set; }
        public double AverageWPM{ get; set; }
        public double AverageAccuracy { get; set; }
        public int NumberOfTests { get; set; }
        public UserStatCatJoin UserStatCatJoin { get; set; }
        public List<TypeTest> TypeTests { get; set; }
    }
}
