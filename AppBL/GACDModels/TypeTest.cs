using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class TypeTest
    {
        public TypeTest() { }
        public int Id { get; set; }
        public int UserStatId { get; set; }
        public UserStat UserStat { get; set; }
        public int NumberOfErrors { get; set; }
        public int NumberOfWords { get; set; }
        public DateTime Date { get; set; }
        public double WPM { get; set; }
    }
}
