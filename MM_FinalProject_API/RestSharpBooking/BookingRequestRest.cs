using BookingModels.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharpBooking.Helpers;
using RestSharpBooking.Resource;
using System.Net;

namespace RestSharpBooking
{
    [TestClass]
    public class BookingRequestRest
    {
        public RestClient restClient { get; set; }
        public List<BookingIdModel> cleanUpList = new List<BookingIdModel>();
        public BookingHelper bookingHelper { get; set; }
        public string token;

        [TestInitialize]
        public async Task Initialize()
        {
            restClient = new RestClient();
            bookingHelper = new BookingHelper();
            token = await bookingHelper.GetToken(restClient);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var postRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "" + data.BookingId)
                .AddHeader("Accept", "application/json")
                .AddHeader("Cookie", $"token={token}");

                var response = await restClient.DeleteAsync(postRequest);
            }
        }

        [TestMethod]
        public async Task CreateBooking()
        {
            //ARRANGE
            BookingInfoModel createModel = new BookingInfoModel
            {
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 1218,
                Depositpaid = true,
                Bookingdates = new BookingDatesModel
                {
                    Checkin = "2023-06-01",
                    Checkout = "2023-06-25"
                },
                Additionalneeds = "Breakfast"
            };

            var postRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint))
               .AddJsonBody(createModel)
               .AddHeader("Accept", "application/json");

            var postResponse= await restClient.ExecutePostAsync<BookingIdModel>(postRequest);
            var postBooking = JsonConvert.DeserializeObject<BookingIdModel>(postResponse.Content);
            
            cleanUpList.Add(postBooking);

            //Act
            var request = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "/" + postBooking.BookingId)
                .AddHeader("Accept", "application/json");
            var getResponse= await restClient.ExecuteGetAsync(request);
            var getBooking = JsonConvert.DeserializeObject<BookingInfoModel>(getResponse.Content);

            //Assert
            Assert.AreEqual(postResponse.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(postBooking.BookingInfo.Firstname, getBooking.Firstname);
            Assert.AreEqual(postBooking.BookingInfo.Lastname, getBooking.Lastname);
            Assert.AreEqual(postBooking.BookingInfo.Totalprice, getBooking.Totalprice);
            Assert.AreEqual(postBooking.BookingInfo.Depositpaid, getBooking.Depositpaid);
            Assert.AreEqual(postBooking.BookingInfo.Bookingdates.Checkout, getBooking.Bookingdates.Checkout);
            Assert.AreEqual(postBooking.BookingInfo.Bookingdates.Checkin, getBooking.Bookingdates.Checkin);
        }


        [TestMethod]
        public async Task UpdateBooking()
        {
            //ARRANGE
            BookingInfoModel createModel = new BookingInfoModel
            {
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 1218,
                Depositpaid = true,
                Bookingdates = new BookingDatesModel
                {
                    Checkin = "2023-06-01",
                    Checkout = "2023-06-25"
                },
                Additionalneeds = "Breakfast"
            };

            var postRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint))
               .AddJsonBody(createModel)
               .AddHeader("Accept", "application/json");

            var postResponse = await restClient.ExecutePostAsync<BookingIdModel>(postRequest);
            var postBooking = JsonConvert.DeserializeObject<BookingIdModel>(postResponse.Content);

            cleanUpList.Add(postBooking);

            var updateBooking = new BookingInfoModel()
            {
                Firstname = "Totoy",
                Lastname = "Brown",
                Totalprice = 2011,
                Depositpaid = true,
                Bookingdates = new BookingDatesModel
                {
                    Checkin = "2023-05-21",
                    Checkout = "2023-09-17"
                },
                Additionalneeds = "Breakfast"
            };

            //Act
            var putRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "/" + postBooking.BookingId)
             .AddJsonBody(updateBooking)
             .AddHeader("Accept", "application/json")
             .AddHeader("Cookie", $"token={token}");
            var updateResponse = await restClient.ExecutePutAsync(putRequest);

            var updateData = JsonConvert.DeserializeObject<BookingInfoModel>(updateResponse.Content);

            var getRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "/" + postBooking.BookingId)
                .AddHeader("Accept", "application/json");
            var getResponse = await restClient.ExecuteGetAsync(getRequest);
            var getBooking = JsonConvert.DeserializeObject<BookingInfoModel>(getResponse.Content);

            //Assert
            Assert.AreEqual(updateResponse.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(updateData.Firstname, getBooking.Firstname);
            Assert.AreEqual(updateData.Lastname, getBooking.Lastname);
            Assert.AreEqual(updateData.Totalprice, getBooking.Totalprice);
            Assert.AreEqual(updateData.Depositpaid, getBooking.Depositpaid);
            Assert.AreEqual(updateData.Bookingdates.Checkout, getBooking.Bookingdates.Checkout);
            Assert.AreEqual(updateData.Bookingdates.Checkin, getBooking.Bookingdates.Checkin);
        }

        [TestMethod]
        public async Task DeleteBooking()
        {
            //ARRANGE
            BookingInfoModel createModel = new BookingInfoModel
            {
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 1218,
                Depositpaid = true,
                Bookingdates = new BookingDatesModel
                {
                    Checkin = "2023-06-01",
                    Checkout = "2023-06-25"
                },
                Additionalneeds = "Breakfast"
            };

            var postRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint))
               .AddJsonBody(createModel)
               .AddHeader("Accept", "application/json");

            var postResponse = await restClient.ExecutePostAsync<BookingIdModel>(postRequest);
            var postBooking = JsonConvert.DeserializeObject<BookingIdModel>(postResponse.Content);

            //Act
            var deleteRequest = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "/" + postBooking.BookingId)
               .AddHeader("Accept", "application/json")
               .AddHeader("Cookie", $"token={token}");

            var response = await restClient.DeleteAsync(deleteRequest);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task InvalidBooking()
        {

            var request = new RestRequest(Endpoints.GetURL(Endpoints.BookingEndpoint) + "/-100010001")
             .AddHeader("Accept", "application/json");
            var response = await restClient.ExecuteGetAsync<BookingInfoModel>(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}