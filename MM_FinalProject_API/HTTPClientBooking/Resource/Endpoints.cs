using BookingModels.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClientBooking.Resource
{
    public class Endpoints
    {
        public static readonly string BaseURL = "https://restful-booker.herokuapp.com/";

        public static readonly string BookingEndpoint = "booking";
        public static readonly string AuthenticateEndpoint = "auth";

        public static string GetURL(string endpoint) => $"{BaseURL}{endpoint}";

        public static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));
    }
}
