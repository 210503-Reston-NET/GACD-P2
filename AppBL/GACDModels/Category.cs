﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GACDModels
{
    public class Category
    {
        public Category() { }
        public int Id { get; set; }
        public int Name { get; set; }
        public List<UserStatCatJoin> UserStatCatJoins { get; set; }
    }
}
