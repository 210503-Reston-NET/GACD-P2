﻿using System;
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
        public int UserCreatedId { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string CompetitionName { get; set; }
        public string TestString { get; set; }

        public List<CompetitionStat> CompetitionStats { get; set; }
    }
}
