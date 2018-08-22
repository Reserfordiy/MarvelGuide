using MarvelGuide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Intefraces
{
    public interface IRoule
    {
        int Id { get; set; }

        string Name { get; set; }

        string Text { get; set; }

        Picture Picture { get; set; }
    }
}
