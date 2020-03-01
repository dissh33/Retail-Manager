using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Library.Api;

namespace RMDesktopUI.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IApiClientInitializer _apiClientInitializer;
        public ProductEndpoint(IApiClientInitializer apiClientInitializer)
        {
            _apiClientInitializer = apiClientInitializer;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.GetAsync("/api/product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
