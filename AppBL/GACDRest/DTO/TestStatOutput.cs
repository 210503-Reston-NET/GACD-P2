using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class TestStatOutput
    {
        public TestStatOutput() { }
        public int numberofcharacters { get; set; }
        public int numberoferrors { get; set; }
        public double wpm { get; set; }
        public int timetakenms { get; set; }
        public DateTime date { get; set; }
    }
}
