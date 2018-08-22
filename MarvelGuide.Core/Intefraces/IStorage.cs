using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Intefraces
{
    public interface IStorage
    {
        IRepository<IRoule> Roules { get; }
    }
}
