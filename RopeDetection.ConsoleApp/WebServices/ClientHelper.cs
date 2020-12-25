using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RopeDetection.ConsoleApp.WebServices
{
    public class ClientHelper
    {
        public const string URL = "https://localhost:5001/api/";

        public static HttpClient GetClient()
        {
            HttpsValidation.Initialize();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient()
            {
                BaseAddress = new Uri(URL),
                Timeout = new TimeSpan(0, 5, 2)
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }

        public static HttpClient GetClient(string username, string password)
        {
            //httpsValidation.Initialize();
            //var handler = new HttpClientHandler
            //{
            //    UseProxy = true,
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            //};

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri(URL),
                Timeout = new TimeSpan(0, 5, 2)
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }

        // Auth with bearer token
        public static HttpClient GetClient(string token)
        {
            //var handler = new HttpClientHandler
            //{
            //    UseProxy = true,
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            //};

            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            var authValue = new AuthenticationHeaderValue("Bearer", token);

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                BaseAddress = new Uri(URL)
                //Set some other client defaults like timeout / BaseAddress
            };

            return client;
        }
    }
}
