using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IApiClientInitializer _apiClientInitializer;
        public SaleEndpoint(IApiClientInitializer apiClientInitializer)
        {
            _apiClientInitializer = apiClientInitializer;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.PostAsJsonAsync("/api/sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //log successful call
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
