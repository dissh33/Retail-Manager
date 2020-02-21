using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RMDesktopUI.Models;

namespace RMDesktopUI.Helpers
{
    class APIHelper : IAPIHelper
    {
        public HttpClient ApiClient { get; set; }

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = "https://localhost:44372";
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(api);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
           // username = JsonConvert.SerializeObject(username);
            //password = JsonConvert.SerializeObject(password);

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

           
            using (HttpResponseMessage response = await ApiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //public async Task<object> GetValue()
        //{
        //    ApiClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(
        //        @"bearer QJCdeeRfRQ3xwn-wi7s929XeI_KJgIqlYtqVB6_fFYVdYIIOPVwTw3Jl7IWh1hTepJ93C0wNNbK7quvFmHv4GC8RADtktulrMwSXobhD8I6EyONcy3LAHi6hGMRpNepU8J9QcrBakc_VCDvOyc5ORoRS05hC8_J5ohClTq4eAMUDcW5N_0Hm_HUhKC_J-KnFRQ3BbYu0JV5ApxEKWX9pWEGgAa82w7M8aeklDtqAGIULBScb_7-S_ok5BNJSUQz59AK2FHkp2FVFDgLGX0cSYQsshkw5C5iztEthj44iYFmwfF5jJyJD2YBz4Tliy-npjW_7umVb6-ERTskPeMBf2duDTTxzd-YQpLHzK9KGsXT9znGDtS-1U9HqOcZ7TQ3qfhanutGr7IMAS-bdSt7eOBTeOEYSXuHd_JvooUbNc3_4a6Ox2JSMHpUaZO6yPcVrxqgRdKDEdfjCI8Nv7OZW7Goozt4hn8Dk1ObYphpdU_s");
        //    using (HttpResponseMessage response = await ApiClient.GetAsync("/api/values"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsAsync<object>();
        //            var test = JsonConvert.SerializeObject(result);
        //            return result; 
        //        }
        //        else
        //        {
        //            var result = await response.Content.ReadAsAsync<string>();
        //            throw new Exception(response.ReasonPhrase);
        //        }
        //    }
        //}

        
    }
}
