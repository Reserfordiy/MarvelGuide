using MarvelGuide.Core.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MarvelGuide.Core.Helpers
{
    public class WorkWithImages
    {
        string _fullImagePath;

        Picture _picture;


        public Picture Picture => _picture;


        public WorkWithImages()
        {
            _picture = new Picture();
        }



        public void UploadImageAndSave()
        {
            if (UploadImage())
            {
                string originDestinationPath = GetDestinationPath(
                    _picture.ImageSource, "../MarvelGuide.Core/Avatars");

                try
                {
                    File.Copy(_fullImagePath, originDestinationPath, false);
                }

                catch
                {
                    int i = 1;

                    string destinationPath;

                    while (true)
                    {
                        destinationPath = AddingASubStringBeforeTypeFormat(originDestinationPath, "(" + i.ToString() + ")");

                        try
                        {
                            File.Copy(_fullImagePath, destinationPath, false);

                            _picture.ImageSource = AddingASubStringBeforeTypeFormat(_picture.ImageSource, "(" + i.ToString() + ")");

                            break;
                        }

                        catch
                        {
                            i++;
                        }
                    }
                }
            }
        }

        private bool UploadImage()
        {
            OpenFileDialog uploadingImageDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Все поддерживаемые форматы|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "PNG (*.png)|*.png"
            };

            if (uploadingImageDialog.ShowDialog() == true)
            {
                _fullImagePath = uploadingImageDialog.FileName;

                string[] partsOfFileName = _fullImagePath.Split('\\');

                _picture.ImageSource = partsOfFileName[partsOfFileName.Length - 1];

                StateOfTheBorderWithDependencyOfType();

                return true;
            }

            return false;
        }

        private void StateOfTheBorderWithDependencyOfType()
        {
            string[] imageNameParts = _picture.ImageSource.Split('.');

            string imageTypeFormat = imageNameParts[imageNameParts.Length - 1];

            if (imageTypeFormat == "png")
            {
                _picture.IsBorderRequired = false;
            }

            else
            {
                _picture.IsBorderRequired = true;
            }
        }



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


        public static string AddingASubStringBeforeTypeFormat(string originalString, string subString)
        {
            string[] imageNameParts = originalString.Split('.');

            string originalStringWithoutTheFormat = "";

            for (int i = 0; i < imageNameParts.Length - 2; i++)
            {
                originalStringWithoutTheFormat += imageNameParts[i] + ".";
            }

            return originalStringWithoutTheFormat + imageNameParts[imageNameParts.Length - 2] + subString + "." + imageNameParts[imageNameParts.Length - 1];
        }
    }
}
