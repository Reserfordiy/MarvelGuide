using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class DBStorage : IStorage
    {
        IRepository<Document> _documents;

        bool _loaded;

        Context _context;


        public IRepository<Document> Documents
        {
            get
            {
                if (_loaded)
                {
                    return _documents;
                }
                
                using (_context = new Context())
                {
                    _documents = new DBRepository<Document>(_context.Roules.ToList());
                    
                    _loaded = true;
                }

                return _documents;
            }
        }
    }
}
