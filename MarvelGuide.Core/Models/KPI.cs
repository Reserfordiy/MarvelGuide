using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class KPI
    {
        public int AmountOfPosts { get; set; }

        public double AverageLikes { get; set; }

        public double AverageLikesEfficiency { get; set; }

        public double AverageReposts { get; set; }

        public double AverageComments { get; set; }
    }
}
