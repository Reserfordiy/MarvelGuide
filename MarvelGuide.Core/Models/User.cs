using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Moderator { get; set; }
        public bool Editor { get; set; }
        public bool Manager { get; set; }

        public virtual Picture Avatar { get; set; }
    }
}
