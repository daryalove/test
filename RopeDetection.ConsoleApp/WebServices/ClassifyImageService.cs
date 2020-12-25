using Newtonsoft.Json;
using RopeDetection.CommonData.ViewModels.TrainViewModel;
using RopeDetection.Shared.DataModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RopeDetection.ConsoleApp.WebServices
{
    public class ClassifyImageService
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
        /// Прогноз модели.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<ModelOutput> PredictSingleImage(PredictModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("ImageClassification/ClassifyImage", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                ModelOutput o_data = new ModelOutput();
                o_data = JsonConvert.DeserializeObject<ModelOutput>(s_result);
                return o_data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
