using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class LBUserModel
    {
        public LBUserModel() { }
        public string UserName { get; set; }
        public string Name { get; set; }
        public double AverageWPM { get; set; }
        public double AverageAcc { get; set; }
    }
}
