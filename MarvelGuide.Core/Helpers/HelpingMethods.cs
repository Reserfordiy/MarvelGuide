using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Helpers
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



        public static bool TryParsingTheDate(string dateString)
        {
            return dateString.Length == 10 && dateString[2] == '.' && dateString[5] == '.' && 
                int.TryParse(dateString.Substring(0, 2), out int result) && int.TryParse(dateString.Substring(3, 2), out result) && 
                int.TryParse(dateString.Substring(6), out result) && DateTime.TryParse(dateString, out DateTime dateTime);
        }
    }
}
