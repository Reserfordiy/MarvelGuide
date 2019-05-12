using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class EditorsPublication
    {
        public string Rubric { get; set; }

        [JsonIgnore]
        public Rubric RubricClass { get; set; }

        public int RubricClassID { get; set; }

        public int Frequency { get; set; }
    }
}
