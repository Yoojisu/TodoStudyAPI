using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoStudy.Models.Filters
{
    public class TodoFilter
    {
        public string Search { get; set; }

        public int Skip { get; set; }

        private int _take { get; set; }

        public int Take { get => _take != 0 ? _take : 10; set => _take = value; }
    }
}
