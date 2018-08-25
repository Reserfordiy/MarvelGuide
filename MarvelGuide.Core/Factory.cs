using MarvelGuide.Core.DataStorages;
using MarvelGuide.Core.Intefraces;
using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core
{
    public class Factory
    {
        private static Factory _instance;

        public static Factory Instance => _instance ?? (_instance = new Factory());

        private Factory() { }

        
        public Document GetDocument() => new Document();
        

        private IStorage _storage;

        public IStorage GetStorage() => _storage ?? (_storage = new DBStorage());
    }
}
