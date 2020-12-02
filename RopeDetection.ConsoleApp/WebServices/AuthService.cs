using Newtonsoft.Json;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.ConsoleApp.Services
{
    public class AuthService
    {
        private static HttpClient _httpClient;

        /// <summary>
        /// Инициализация экземпляра клиента
        /// </summary>
        /// <param name="client"></param>
        public static void InitializeClient(HttpClient client)
        {
            _httpClient = client;
        }


        /// <summary>
        /// Выполнение входа.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<UserShortModel> Login(string username, string userPassword)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "username", username },
                        { "userpassword", userPassword},
                    });

                var password = WebUtility.UrlEncode(userPassword);
                HttpResponseMessage response = await _httpClient.PostAsync($"auth/loginuser?username={username}&userpassword={userPassword}", formContent);
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                UserShortModel o_data = new UserShortModel();
                o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
                return o_data;
            }
            catch (Exception ex)
            {
                UserShortModel o_data = new UserShortModel();
                o_data.Error = ex;
                o_data.Result = CommonData.DefaultEnums.Result.Error;
                return o_data;
            }
        }

        public static async Task<UserShortModel> LogOut()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"auth/logoutuser");
                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                MessageResponse o_data = new MessageResponse();
                UserShortModel model = new UserShortModel();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            o_data = JsonConvert.DeserializeObject<MessageResponse>(s_result);
                            model.SuccessInfo = o_data.message;
                            model.Result = CommonData.DefaultEnums.Result.OK;
                            return model;
                        }
                    default:
                        {
                            throw new Exception("Ошибка выхода. Попробуйте снова.");
                        }
                }
            }
            catch (Exception ex)
            {
                UserShortModel o_data = new UserShortModel();
                o_data.Error = ex;
                o_data.Result = CommonData.DefaultEnums.Result.Error;
                return o_data;
            }
        }


        /// <summary>
        /// Регистрация физического лица.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<UserShortModel> Register(UserModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("auth/registeruser", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                UserShortModel o_data = new UserShortModel();
                o_data = JsonConvert.DeserializeObject<UserShortModel>(s_result);
                return o_data;
            }
            catch (Exception ex)
            {
                UserShortModel o_data = new UserShortModel();
                o_data.Error = ex;
                o_data.Result = CommonData.DefaultEnums.Result.Error;
                return o_data;
            }
        }
    }
}
