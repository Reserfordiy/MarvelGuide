using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Intefraces
{
    public interface IStorage
    {
        IRepository<Document> Documents { get; }
    }
}
