using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class Rubric
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Actual { get; set; }

        [JsonIgnore]
        public Document Document { get; set; }

        public int DocumentId { get; set; }

        public KPI KPI { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual Picture PictureDark { get; set; }
    }
}
