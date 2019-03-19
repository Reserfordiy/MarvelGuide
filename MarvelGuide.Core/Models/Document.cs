using MarvelGuide.Core.Intefraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreationDate { get; set; }

        public int PreviousVersionId { get; set; }       
    }
}
