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
        IRepository<User> _users;
        IRepository<Rubric> _rubrics;

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
                    _documents = new DBRepository<Document>(_context.Documents.ToList());
                    _users = new DBRepository<User>(_context.Users.ToList());
                    _rubrics = new DBRepository<Rubric>(_context.Rubrics.ToList());

                    _loaded = true;

                    //SaveDocumentsToJson();
                    //SaveUsersToJson();
                }                

                return _documents;
            }
        }

        public IRepository<User> Users => _users;
        public IRepository<Rubric> Rubrics => _rubrics;



        public void ChangingUsersModels()
        {
            
        }
        public void ChangingDocumentsModels()
        {

        }
        public void ChangingRubricsModels()
        {

        }



        public void SaveDocumentsToJson()
        {
            using (var sw = new StreamWriter("../../../MarvelGuide.Core/Data/Documents.json"))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, _documents.Items);
                }
            }
        }

        public void SaveUsersToJson()
        {
            using (var sw = new StreamWriter("../../../MarvelGuide.Core/Data/Users.json"))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, _users.Items);
                }
            }
        }
    }
}
