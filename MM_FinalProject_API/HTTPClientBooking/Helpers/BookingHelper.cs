using BookingModels.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HTTPClientBooking.Resource;

namespace HTTPClientBooking.Helpers
{
    public class BookingHelper
    {
        public async Task<string> GetToken(HttpClient httpClient)
        {
            var loginData = new UserModel()
            {
                Username = "admin",
                Password = "password123"
            };

            var payload = JsonConvert.SerializeObject(loginData);
            var postRequest = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(Endpoints.GetURL(Endpoints.AuthenticateEndpoint), postRequest);

            var tokenResponse = JsonConvert.DeserializeObject<TokenModel>(response.Content.ReadAsStringAsync().Result);

            return tokenResponse.Token;
        }
    }
}
