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
        IRepository<IDocument> _roules;


        public IRepository<IDocument> Roules
        {
            get
            {
                return _roules;
            }
        }
    }
}
