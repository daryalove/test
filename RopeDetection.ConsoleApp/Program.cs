using RopeDetection.CommonData.ViewModels.LabelViewModel;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.ConsoleApp.Services;
using RopeDetection.ConsoleApp.WebServices;
using RopeDetection.Train;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RopeDetection.ConsoleApp
{
    class Program
    {
        private static string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../../"));
        private static string testImageOneRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.WpfApp", "TestImages");
        //private static string testImageSecondRelativePath = System.IO.Path.Combine(projectDirectory, "RopeDetection.WpfApp", "TestImages", "i70012.png");

        static void Main(string[] args)
        {
            try
            {
                UserModel model = new UserModel
                {
                    UserFIO = "Илья Головко",
                    Description = "",
                    Email = "ilya@mail.ru",
                    IsAllowed = true,
                    Password = "ilya_123",
                    UserName = ""
                };

                bool showMenu = true;
                while (showMenu)
                {
                    showMenu = MainMenu(model);
                }
  
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
            }
        }

        private static bool MainMenu(UserModel model)
        {

            Console.Clear();
            Console.WriteLine("=====================");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(1) Login User");
            Console.WriteLine("(2) Register User");
            Console.WriteLine("(3) Upload Files");
            Console.WriteLine("(4) Log Out Profile");
            Console.WriteLine("(5) Start Training");
            Console.WriteLine("(6) Exit App");
            Console.WriteLine("=====================");
            Console.Write("\r\nSelect an option: ");
            
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        LoginUser(model.Email, model.Password).Wait();
                        System.Threading.Thread.Sleep(30000);
                        return true;
                    }
                case "2":
                    {
                        RegisterUser(model).Wait();
                        System.Threading.Thread.Sleep(30000);
                        return true;
                    }
                case "3":
                    {
                        UploadTrainingFiles().Wait();
                        System.Threading.Thread.Sleep(30000);
                        return true;
                    }
                case "4":
                    {
                        LogOut().Wait();
                        System.Threading.Thread.Sleep(30000);
                        return true;
                    }
                case "5":
                    {
                        Console.Clear();
                        TrainModel();
                        System.Threading.Thread.Sleep(30000);
                        return true;
                    }
                case "6":
                    {
                        return false;
                    }
                default:
                    return true;
            }
        }

        private static void TrainModel()
        {
            Console.WriteLine("Starting training...");
            // Measure #1 prediction execution time.
            var watch = System.Diagnostics.Stopwatch.StartNew();

            ModelBuilder.CreateModel();

            // Stop measuring time.
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            var minutes = TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes;
            Console.WriteLine("First Training took: " + minutes + " minutes");
        }

        private async static Task UploadTrainingFiles()
        {
            List<FileModel> files = new List<FileModel>();

            var filesFromDirectory = Directory.GetFiles(testImageOneRelativePath, "*",
searchOption: SearchOption.AllDirectories);

            foreach (var file in filesFromDirectory)
            {
                files.Add(new FileModel
                {
                    FileContent = File.ReadAllBytes(file),
                    FileName = Path.GetFileName(file),
                    FileType = System.IO.Path.GetExtension(file)
                });
            }

            CreateFilesModel model = new CreateFilesModel
            {
                Files = files,
                ModelId = Guid.Parse("6E873C5E-9EC9-4182-B112-98F86A76106A"),
                TypeId = Guid.Parse("69FA14B0-4FEF-4495-B2E6-04158F6EA7C4")
            };

            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                ModelService.InitializeClient(client);
                var o_data = await ModelService.UploadTrainingFiles(model);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is error: " + o_data.ErrorInfo);
            }
        }


        private async static Task LogOut()
        {
            using (var client = ClientHelper.GetClient(StaticUser.Token))
            {
                AuthService.InitializeClient(client);
                var o_data = await AuthService.LogOut();

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine(o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is error: " + o_data.ErrorInfo);
            }
        }

        private async static Task LoginUser(string userName, string password)
        {
            using (var client = ClientHelper.GetClient())
            {
                AuthService.InitializeClient(client);
                UserShortModel o_data = null;
                o_data = await AuthService.Login(userName, password);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine("User: " + o_data.UserFIO);
                    Console.WriteLine("Token: " + o_data.Token);
                    StaticUser.Token = o_data.Token;
                }
                else
                    Console.WriteLine("Something is error: " + o_data.ErrorInfo);
            }
        }

        private async static Task RegisterUser(UserModel model)
        {
            using (var client = ClientHelper.GetClient())
            {
                AuthService.InitializeClient(client);
                UserShortModel o_data = null;
                o_data = await AuthService.Register(model);

                if (o_data.Result.ToString() == "OK")
                {
                    Console.WriteLine("User: " + o_data.UserFIO);
                    Console.WriteLine("Info: " + o_data.SuccessInfo);
                }
                else
                    Console.WriteLine("Something is error: " + o_data.ErrorInfo);
            }
        }
    }
}
