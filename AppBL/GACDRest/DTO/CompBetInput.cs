using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class CompBetInput
    {
        public CompBetInput() { }
        public int UserBetOn { get; set; }
        public int CompId { get; set; }
        public int BetAmount { get; set; }
    }
}
