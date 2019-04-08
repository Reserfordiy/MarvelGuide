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
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Rubric> Rubrics { get; set; }


        public Context() : base("MarvelGuideDB") { }
    }
}
