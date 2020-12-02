using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RopeDetection.Shared
{
    public class FileUtils
    {
        public static IEnumerable<ImageData> LoadImagesFromDirectory(string folder, bool useFolderNameAsLabel = true)
        {
            var files = Directory.GetFiles(folder, "*",
    searchOption: SearchOption.AllDirectories);

            foreach (var file in files)
            {
                //if(LTP == 1)
                //{
                //    if((Path.GetFileName(file) != "7001-21.jpg") )
                //    {
                //        continue;
                //    }
                //}
                if ((Path.GetExtension(file) != ".jpg") && (Path.GetExtension(file) != ".png"))
                    continue;

                var label = Path.GetFileName(file);

                if (useFolderNameAsLabel)
                    label = Directory.GetParent(file).Name;
                else
                {
                    for (int index = 0; index < label.Length; index++)
                    {
                        if (!char.IsLetter(label[index]))
                        {
                            label = label.Substring(0, index);
                            break;
                        }
                    }
                }

                yield return new ImageData()
                {
                    ImagePath = file,
                    Label = label
                };
            }
        }

        public static string GetAbsolutePath(Assembly assembly, string relativePath)
        {
            var assemblyFolderPath = new FileInfo(assembly.Location).Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }
    }
}
