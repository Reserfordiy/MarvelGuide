using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.SpecialMethods
{
    public static class HelpingMethods
    {
        public static string ChoosingTheCorrespondingEnding(string ending1, string ending234, string ending5, int number)
        {
            string stringNumber = number.ToString();

            if (stringNumber.Length == 1)
            {
                if (number == 1) { return ending1; }
                if (number == 2 || number == 3 || number == 4) { return ending234; }
                return ending5;
            }
            else if (stringNumber.Length == 2)
            {
                if (stringNumber[0] == '1') { return ending5; }
                if (stringNumber[1] == '1') { return ending1; }
                if (stringNumber[1] == '2' || stringNumber[1] == '3' || stringNumber[1] == '4') { return ending234; }
                return ending5;
            }
            else
            {
                return ending5;
            }
        }
    }
}
