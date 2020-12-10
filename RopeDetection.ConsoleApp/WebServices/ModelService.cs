using Newtonsoft.Json;
using RopeDetection.CommonData.ViewModels.Base;
using RopeDetection.CommonData.ViewModels.LabelViewModel;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.ConsoleApp.WebServices
{
    public class ModelService
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
        /// Загрузка файлов.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<BaseModel> UploadTrainingFiles(CreateFilesModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("model/uploadfilesfortraining", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                BaseModel o_data = new BaseModel();
                o_data = JsonConvert.DeserializeObject<BaseModel>(s_result);
                return o_data;
            }
            catch (Exception ex)
            {
                BaseModel o_data = new BaseModel();
                o_data.Error = ex;
                o_data.Result = CommonData.DefaultEnums.Result.Error;
                return o_data;
            }
        }

    }
}
