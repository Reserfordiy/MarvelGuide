using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class JSONRepository<T> : BaseRepository<T>
    {
        string _fileName;


        public JSONRepository(string fileName)
        {
            _fileName = fileName;

            Restore();
        }


        private void Restore()
        {
            try
            {
                using (var sr = new StreamReader(_fileName))
                {
                    using (var jsonReader = new JsonTextReader(sr))
                    {
                        var serializer = new JsonSerializer();
                        _items = serializer.Deserialize<List<T>>(jsonReader);
                    }
                }
            }

            catch
            {
                _items = new List<T>();
            }
        }


        public override void Save()
        {
            using (var sw = new StreamWriter(_fileName))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, _items);
                }
            }
        }
    }
}
