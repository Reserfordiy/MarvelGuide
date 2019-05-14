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
        [JsonIgnore]
        public Rubric Rubric { get; set; }

        public int RubricID { get; set; }

        public int Frequency { get; set; }
    }
}
