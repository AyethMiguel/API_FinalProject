using BookingModels.DataModels;
using HTTPClientBooking.Helpers;
using HTTPClientBooking.Resource;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace HTTPClientBooking
{
    [TestClass]
    public class BookingRequestHttp
    {
        public BookingHelper BookingHelper { get; set; }
        private static HttpClient httpClient;
        private readonly List<BookingIdModel> cleanUpList = new List<BookingIdModel>();

        [TestInitialize]
        public async Task TestInitialize()
        {
            httpClient = new HttpClient();
            BookingHelper = new BookingHelper();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={ await BookingHelper.GetToken(httpClient)}");
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(Endpoints.GetURL($"{Endpoints.BookingEndpoint}/{data.BookingId}"));
            }
        }

        [TestMethod]
        public async Task CreateBooking()
        {
            #region Create data
            //ARRANGE
            BookingInfoModel bookingData = new BookingInfoModel()
            {
                Additionalneeds = "Breakfast",
                Bookingdates = new BookingDatesModel()
                {
                    Checkin = "2013-02-23",
                    Checkout = "2014-10-23"
                },
                Depositpaid = true,
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 111,
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(bookingData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");
            #endregion

            #region Send post request to update data
            //ACT
            // Post Request
            var postResponse = await httpClient.PostAsync(Endpoints.GetURL(Endpoints.BookingEndpoint), postRequest);
            var postData = JsonConvert.DeserializeObject<BookingIdModel>(postResponse.Content.ReadAsStringAsync().Result);
            #endregion

            #region Get posted data
            // Get Request
            var getResponse = await httpClient.GetAsync(Endpoints.GetURI($"{Endpoints.BookingEndpoint}/{postData.BookingId}"));
            var listBookingInfoModel = JsonConvert.DeserializeObject<BookingInfoModel>(getResponse.Content.ReadAsStringAsync().Result);
            #endregion

            #region Assertion
            //ASSERT
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode, "Status code is not equal to 200");
            #endregion

            #region Cleanup data
            // Add data to cleanup list
            cleanUpList.Add(postData);
            #endregion
        }

        [TestMethod]
        public async Task UpdateBooking()
        {
            #region Create data
            //ARRANGE
            BookingInfoModel bookingData = new BookingInfoModel()
            {
                Additionalneeds = "Breakfast",
                Bookingdates = new BookingDatesModel()
                {
                    Checkin = "2013-02-23",
                    Checkout = "2014-10-23"
                },
                Depositpaid = true,
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 111,
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(bookingData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");
            #endregion

            #region Send post request to update data
            //ACT
            // Post Request
            var httpResponse = await httpClient.PostAsync(Endpoints.GetURL(Endpoints.BookingEndpoint), postRequest);
            var postData = JsonConvert.DeserializeObject<BookingIdModel>(httpResponse.Content.ReadAsStringAsync().Result);

            var updateBooking = new BookingInfoModel()
            {
                Firstname = "Totoy",
                Lastname = "Brown",
                Totalprice = 521,
                Depositpaid = true,
                Bookingdates = new BookingDatesModel
                {
                    Checkin = "2023-05-21",
                    Checkout = "2023-09-17"
                },
                Additionalneeds = "Breakfast"
            };

            request = JsonConvert.SerializeObject(updateBooking);
            var putRequest = new StringContent(request, Encoding.UTF8, "application/json");
            var updateResponse = await httpClient.PutAsync(Endpoints.GetURI($"{Endpoints.BookingEndpoint}/{postData.BookingId}"), putRequest);
            var updateData = JsonConvert.DeserializeObject<BookingInfoModel>(updateResponse.Content.ReadAsStringAsync().Result);
            #endregion

            #region Get posted data
            // Get Request
            var getResponse = await httpClient.GetAsync(Endpoints.GetURI($"{Endpoints.BookingEndpoint}/{postData.BookingId}"));
            var getData = JsonConvert.DeserializeObject<BookingInfoModel>(getResponse.Content.ReadAsStringAsync().Result);
            #endregion

            #region Assertion
            //ASSERT
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode, "Status code is not equal to 200");
            Assert.AreEqual(updateData.Firstname, getData.Firstname);
            Assert.AreEqual(updateData.Lastname, getData.Lastname);
            Assert.AreEqual(updateData.Totalprice, getData.Totalprice);
            Assert.AreEqual(updateData.Depositpaid, getData.Depositpaid);
            Assert.AreEqual(updateData.Bookingdates.Checkout, getData.Bookingdates.Checkout);
            Assert.AreEqual(updateData.Bookingdates.Checkin, getData.Bookingdates.Checkin); ;
            #endregion
            cleanUpList.Add(postData);
        }

        [TestMethod]
        public async Task DeleteBooking()
        {
            #region Create data

            //ARRANGE
            BookingInfoModel bookingData = new BookingInfoModel()
            {
                Additionalneeds = "Breakfast",
                Bookingdates = new BookingDatesModel()
                {
                    Checkin = "2013-02-23",
                    Checkout = "2014-10-23"
                },
                Depositpaid = true,
                Firstname = "Sally",
                Lastname = "Brown",
                Totalprice = 111,
            };

            var request = JsonConvert.SerializeObject(bookingData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");
            #endregion

            var httpResponse = await httpClient.PostAsync(Endpoints.GetURL(Endpoints.BookingEndpoint), postRequest);
            var postData = JsonConvert.DeserializeObject<BookingIdModel>(httpResponse.Content.ReadAsStringAsync().Result);

            //ACT
            var deleteResponse = await httpClient.DeleteAsync(Endpoints.GetURL($"{Endpoints.BookingEndpoint}/{postData.BookingId}"));

            #region Assertion
            //ASSERT
            Assert.AreEqual(deleteResponse.StatusCode, HttpStatusCode.Created);
            #endregion
        }

        [TestMethod]
        public async Task InvalidBooking()
        {
            #region Get posted data
            // Get Request
            var getResponse = await httpClient.GetAsync(Endpoints.GetURI($"{Endpoints.BookingEndpoint}/{-100010001}"));
            #endregion

            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.NotFound);
        }
    }
}