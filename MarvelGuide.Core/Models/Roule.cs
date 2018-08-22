using MarvelGuide.Core.Intefraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    class Roule : IRoule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public virtual Picture Picture { get; set; }        
    }
}
