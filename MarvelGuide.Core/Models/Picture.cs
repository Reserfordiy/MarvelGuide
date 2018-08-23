using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class Picture
    {
        public int Id { get; set; }

        public string ImageSource { get; set; }

        public bool IsBorderRequired { get; set; }


        public Picture(string imageSource, bool isBorderRequired)
        {
            ImageSource = imageSource;
            IsBorderRequired = isBorderRequired;
        }

        public Picture() { }
    }
}
