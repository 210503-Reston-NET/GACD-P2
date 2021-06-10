using GACDModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest.DTO
{
    public class TypeTestInput : TypeTest
    {
        public TypeTestInput() { }
        public int CategoryId { get; set; }
        
    }
}
