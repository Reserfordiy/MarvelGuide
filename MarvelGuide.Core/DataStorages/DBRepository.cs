using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal class DBRepository<T> : BaseRepository<T>
    {
        public DBRepository(List<T> items)
        {
            _items = items;
        }

        public override void Save() { }
    }
}
