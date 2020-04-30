using MarvelGuide.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Models
{
    public class EditorsPublication
    {
        [JsonIgnore]
        public Rubric Rubric { get; set; }

        public int RubricID { get; set; }

        public int Frequency { get; set; }


        public string StringFrequency()
        {
            if (Frequency == 1)
            {
                return "1 пост каждый день";
            }
            else
            {
                var correspondingEnding = HelpingMethods.ChoosingTheCorrespondingEnding(" день", " дня", " дней", Frequency);

                return "1 пост раз в " + Frequency.ToString() + correspondingEnding;
            }
        }
    }
}
