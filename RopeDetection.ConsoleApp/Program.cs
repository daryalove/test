using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.ConsoleApp.Services;
using RopeDetection.ConsoleApp.WebServices;
using RopeDetection.Train;
using System;
using System.Threading.Tasks;

namespace RopeDetection.ConsoleApp
{
    class Program
    {
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

                LoginUser(model.Email, model.Password).Wait();
                System.Threading.Thread.Sleep(20000);
                LogOut().Wait();
                //Console.WriteLine("Starting training...");
                //// Measure #1 prediction execution time.
                //var watch = System.Diagnostics.Stopwatch.StartNew();

                //ModelBuilder.CreateModel();

                //// Stop measuring time.
                //watch.Stop();

                //var elapsedMs = watch.ElapsedMilliseconds;
                //var minutes = TimeSpan.FromMilliseconds(elapsedMs).TotalMinutes;
                //Console.WriteLine("First Training took: " + minutes + " minutes");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something is wrong: " + ex.Message);
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
