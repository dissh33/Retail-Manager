using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public class Authentication : IAuthentication
    {
        private readonly IApiClientInitializer _apiClientInitializer;
        private readonly ILoggedInUserModel _loggedInUser;

        public Authentication(IApiClientInitializer apiClientInitializer, ILoggedInUserModel loggedInUser)
        {
            _apiClientInitializer = apiClientInitializer;
            _loggedInUser = loggedInUser;
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = JsonConvert.SerializeObject(new
            {
                Grant_Type = "password",
                UserName = username,
                Password = password
            });

            var buffer = Encoding.UTF8.GetBytes(data);
            var byteContent = new ByteArrayContent(buffer);

            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.PostAsync("/token", byteContent))
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

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClientInitializer.ApiClient.DefaultRequestHeaders.Clear();
            _apiClientInitializer.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClientInitializer.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClientInitializer.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClientInitializer.ApiClient.GetAsync("/api/user"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
