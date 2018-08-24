using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class JSONStorage : IStorage
    {
        private const string filePath = "../../../MarvelGuide.Core/Data/Documents.json";


        IRepository<Document> _documents;

        bool _loaded;


        public IRepository<Document> Documents
        {
            get
            {
                if (_loaded)
                {
                    return _documents;
                }

                _documents = new JSONRepository<Document>(filePath);

                return _documents;
            }
        }
    }
}
