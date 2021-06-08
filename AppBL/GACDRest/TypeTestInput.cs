using GACDModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GACDRest
{
    public class TypeTestInput : TypeTest
    {
        public TypeTestInput() { }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
