using GACDModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class TypeTestInput 
    {
        public TypeTestInput() { }
        public int Category { get; set; }
        public int Errors { get; set; }
        public int CharTyped { get; set; }
        public int TimeTaken { get; set; }
        public DateTime Date { get; set; }
    }
}
