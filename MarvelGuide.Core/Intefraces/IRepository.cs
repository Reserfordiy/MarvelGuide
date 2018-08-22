using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Intefraces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Items { get; }

        void Add(T item);

        void Remove(T item);

        void Save();
    }
}
