﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class User
    {
        public User() { }
        public int  Id { get; set; }
        public string Auth0Id { get; set; }
        public int Revapoints { get; set; }
        public List<UserStatCatJoin> UserStatCatJoins { get; set; }
        public List<CompetitionStat> CompetitionStats { get; set; }
    }
}
