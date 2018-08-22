using MarvelGuide.Core.Intefraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class DBStorage : IStorage
    {
        IRepository<IRoule> _roules;


        public IRepository<IRoule> Roules
        {
            get
            {
                return _roules;
            }
        }
    }
}
