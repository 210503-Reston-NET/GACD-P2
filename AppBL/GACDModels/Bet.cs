using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class Bet
    {
        public Bet() { }
        public int Id{ get; set; }
        public int BettingUserId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        [ForeignKey("UserId,CompetitionId")]
        public CompetitionStat CompetitionStat { get; set; }
        public int PointsBet { get; set; }
        public bool Won { get; set; }
        public bool Claimed { get; set; }

    }
}
