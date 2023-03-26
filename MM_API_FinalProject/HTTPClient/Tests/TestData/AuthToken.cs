using HTTPClientBooking.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClientBooking.Tests.TestData
{
    public class AuthToken
    {
        public static TokenModel()
        {
            new TokenModel
            {
                Username = "admin",
                Password = "password123"
            };
        }
    }
}
