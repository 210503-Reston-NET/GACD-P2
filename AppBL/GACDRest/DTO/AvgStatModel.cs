using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class AvgStatModel
    {
        public AvgStatModel() { }
        public int userID { get; set; }
        public double averagewpm { get; set; }
        public double averageaccuracy { get; set; }
        public int numberoftests { get; set; }
        public int totaltesttime { get; set; }
        public int revapoints { get; set; }
    }
}
