using MarvelGuide.Core.Intefraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.DataStorages
{
    internal abstract class BaseRepository<T> : IRepository<T>
    {
        protected List<T> _items;
        public IEnumerable<T> Items => _items;
        
        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public abstract void Save();
    }
}
