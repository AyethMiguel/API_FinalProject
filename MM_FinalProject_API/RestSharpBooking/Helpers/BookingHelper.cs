using BookingModels.DataModels;
using Newtonsoft.Json;
using RestSharp;
using RestSharpBooking.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpBooking.Helpers
{
    public class BookingHelper
    {
        public async Task<string> GetToken(RestClient restClient)
        {
            var loginData = new UserModel()
            {
                Username = "admin",
                Password = "password123"
            };

            var request = new RestRequest(Endpoints.GetURL(Endpoints.AuthenticateEndpoint))
               .AddJsonBody(loginData)
               .AddHeader("Accept", "application/json");

            var response = await restClient.ExecutePostAsync(request);
            var content = JsonConvert.DeserializeObject<TokenModel>(response.Content);

            return content.Token;
        }
    }
}
