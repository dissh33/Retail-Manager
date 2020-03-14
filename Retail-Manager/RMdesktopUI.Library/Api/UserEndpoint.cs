using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiClientInitializer _apiClientInitializer;

        public UserEndpoint(IApiClientInitializer apiClientInitializer)
        {
            _apiClientInitializer = apiClientInitializer;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.GetAsync("/api/User/Admin/GetAllUsers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.GetAsync("/api/User/Admin/GetAllRoles"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return result;
                }
                else throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task AddUserToRole(UserRolePairModel pairing)
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.PostAsJsonAsync("/api/User/Admin/AddRole", pairing))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Send feedback about successful operation
                }
                else throw new Exception(response.ReasonPhrase);
            }
        }
        public async Task RemoveUserFromRole(UserRolePairModel pairing)
        {
            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.PostAsJsonAsync("/api/User/Admin/RemoveRole", pairing))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Send feedback about successful operation
                }
                else throw new Exception(response.ReasonPhrase);
            }
        }

    }
}
