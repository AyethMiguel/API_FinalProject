using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClientBooking.Resource
{
    public class Endpoints
    {
       
        public static readonly string BookingEndpoint = "booking";
        private static readonly string BaseURL = "https://restful-booker.herokuapp.com";

        public static string GetURL(string endpoint) => $"{BaseURL}{endpoint}";
        public static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));
        public static string Authenticate() => $"{BaseURL}/auth";
    }
}
