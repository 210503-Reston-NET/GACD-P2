using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class Competition
    {
        public Competition() { }
        public int Id { get; set; }
        public int NumberOfParticipants { get; set; }
        public List<CompetitionStat> CompetitionStats { get; set; }
    }
}
