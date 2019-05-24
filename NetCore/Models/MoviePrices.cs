using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Models
{
    public class MoviePrices
    {
        public int Id { get; set; }

        public string MovieName { get; set; }

        public decimal? Price { get; set; }
    }
}
