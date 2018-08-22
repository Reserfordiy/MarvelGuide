using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MarvelGuide.Core.Models;

namespace MarvelGuide.Core
{
    public class Context : DbContext
    {
        public DbSet<Roule> Roules { get; set; }

        public DbSet<Picture> Pictures { get; set; }


        public Context() : base("MarvelGuideDB") { }
    }
}
