using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClientBooking.Tests.TestData
{
    public class GenerateBooking
    {
        public static BookingInfoModel createBooking()
        {
            return new BookingInfoModel
            {
                Firstname = "Ayeth",
                Lastname = "Miguel",
                Totalprice = 1025,
                Depositpaid = true,
                Bookingdates = new List<BookingInfoModel>()
                {
                    Checkin = "2023-04-01",
                    Checkout = "2023-04-02"
                },
                Additionalneeds = "Breakfast"
            };
        }
    }
}
