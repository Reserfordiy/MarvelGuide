using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

                //SaveDocumentsToJson();

                return _documents;
            }
        }


        //public void SaveDocumentsToJson()
        //{
        //    using (var sw = new StreamWriter("../../../MarvelGuide.Core/Data/Documents.json"))
        //    {
        //        using (var jsonWriter = new JsonTextWriter(sw))
        //        {
        //            jsonWriter.Formatting = Formatting.Indented;

        //            var serializer = new JsonSerializer();
        //            serializer.Serialize(jsonWriter, _documents.Items);
        //        }
        //    }
        //}
    }
}
