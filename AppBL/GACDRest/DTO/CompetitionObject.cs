using System;
using System.Collections.Generic;


namespace GACDRest.DTO{

    public class CompetitionObject
    {
        
        public CompetitionObject() { }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Category { get; set; }
        
    }

}