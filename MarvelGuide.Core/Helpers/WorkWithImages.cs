using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelGuide.Core.Helpers
{
    public static class WorkWithImages
    {
        public static string GetDestinationPath(string fileName, string folderName)
        {
            string appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            for (int i = 0; i < 2; i++)
            {
                int lastIndex = appStartPath.LastIndexOf("\\");
                appStartPath = new string(appStartPath.Take(lastIndex).ToArray());
            }

            appStartPath = string.Format(appStartPath + "\\{0}\\" + fileName, folderName);

            return appStartPath;
        }
    }
}
