using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;


namespace RMDesktopUI.Library.Api
{
    public class ApiClientInitializer : IApiClientInitializer
    {

        public HttpClient ApiClient { get; private set; }

        public ApiClientInitializer()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(api);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
