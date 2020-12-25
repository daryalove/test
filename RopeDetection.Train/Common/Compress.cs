using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RopeDetection.Train.Common
{
    public class Compress
    {
        private static string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static string assetsRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.Train", "assets");
        private static string workspaceRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.Train", "workspace");

        /// <summary>
        /// Извлечение файлов
        /// </summary>
        /// <param name="gzipFileName"></param>
        /// <param name="targetDir"></param>
        public static void ExtractGZip(string gzipFileName, string targetDir)
        {
            // Use a 4K buffer. Any larger is a waste.    
            byte[] dataBuffer = new byte[4096];

            using (System.IO.Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
            {
                using (GZipInputStream gzipStream = new GZipInputStream(fs))
                {
                    // Change this to your needs
                    string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (FileStream fsOut = File.Create(fnOut))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }

        /// <summary>
        /// Сохранение файлов для обучения
        /// </summary>
        /// <param name="zipItems"></param>
        /// <returns></returns>
        public static bool SaveFilesInFolder(IEnumerable<ImageByteContent> zipItems)
        {
            try
            {
                //Delete all files in a directory    
                string[] dirs = Directory.GetDirectories(assetsRelativePath);
                foreach (string dir in dirs)
                {
                    Directory.Delete(dir, true);
                    Console.WriteLine($"{dir} is deleted.");
                }

                //Empty workspace
                Directory.Delete(workspaceRelativePath, true);
                Directory.CreateDirectory(workspaceRelativePath);

                //Save files in directory
                foreach (var zipItem in zipItems)
                {
                    var path = Path.Combine(assetsRelativePath, zipItem.Label, zipItem.ImageName);
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Path.Combine(assetsRelativePath, zipItem.Label));
                    if (!directory.Exists)
                        directory.Create();

                    File.WriteAllBytes(path, zipItem.ImageContent);
                }
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Архивация файлов
        /// </summary>
        /// <param name="zipItems"></param>
        /// <returns></returns>
        public static Stream Zip(IEnumerable<ImageByteContent> zipItems)
        {
            var zipStream = new MemoryStream();

            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var zipItem in zipItems)
                {
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(zipItem.Label);
                    var entry = zip.CreateEntry(Path.Combine(directory.FullName, zipItem.ImageName, zipItem.FileType));
              
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(zipItem.ImageContent, 0, zipItem.ImageContent.Length);
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }

        /// <summary>
        /// Извлечение файлов из архива
        /// </summary>
        /// <param name="gzArchiveName"></param>
        /// <param name="destFolder"></param>
        public static void UnZip(String gzArchiveName, String destFolder)
        {
            var flag = gzArchiveName.Split(Path.DirectorySeparatorChar).Last().Split('.').First() + ".bin";
            if (File.Exists(Path.Combine(destFolder, flag))) return;

            Console.WriteLine($"Extracting.");
            var task = Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(gzArchiveName, destFolder);
            });

            while (!task.IsCompleted)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }

            File.Create(Path.Combine(destFolder, flag));
            Console.WriteLine("");
            Console.WriteLine("Extracting is completed.");
        }

        /// <summary>
        /// Извлечение файлов из архива
        /// </summary>
        /// <param name="gzArchiveName"></param>
        /// <param name="destFolder"></param>
        public static void ExtractTGZ(String gzArchiveName, String destFolder)
        {
            var flag = gzArchiveName.Split(Path.DirectorySeparatorChar).Last().Split('.').First() + ".bin";
            if (File.Exists(Path.Combine(destFolder, flag))) return;

            Console.WriteLine($"Extracting.");
            var task = Task.Run(() =>
            {
                using (var inStream = File.OpenRead(gzArchiveName))
                {
                    using (var gzipStream = new GZipInputStream(inStream))
                    {
                        using (TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
                            tarArchive.ExtractContents(destFolder);
                    }
                }
            });

            while (!task.IsCompleted)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }

            File.Create(Path.Combine(destFolder, flag));
            Console.WriteLine("");
            Console.WriteLine("Extracting is completed.");
        }
    }
}
