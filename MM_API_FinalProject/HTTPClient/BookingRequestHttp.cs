using HTTPClientBooking;
using HTTPClientBooking.Helpers;
using System.Net;

namespace HTTPClient

    [TestClass]
public class BookingRequestHttp
{
    public HttpClient httpClient;

    public BookingHelper bookingHelper;

    private readonly List<BookingInfoModel> cleanUpList = new List<BookingInfoModel>();

    private static readonly string BaseURL = "https://restful-booker.herokuapp.com/";

    private static readonly string BookingEndpoint = "booking";

    [TestInitialize]
    public async Task TestInitialize()
    {
        httpClient = new HttpClient();
        bookingHelper = new BookingHelper();
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        foreach (var data in cleanUpList)
        {
            var httpResponse = await httpClient.DeleteAsync(GetURL($"{BookingEndpoint}/{data.Id}"));
        }
    }
    /// <summary>
    /// Create a test method that creates a booking
    /// </summary>
    [TestMethod]
    public async Task CreateBookingR()
    {
        #region Create user
        // Create user
        var newBook = new BookingInfoModel()
        {
            Firstname = "Aya",
            Lastname = "Shino",
            Totalprice = 987,
            Depositpaid = true,
            Bookingdates = new List<BookingInfoModel>()
            {
                Checkin = "2023-04-01",
                Checkout = "2024-04-01"
            },
            Additionalneeds = "Breakfast"
        };

        // Send Post Request
        var postRestRequest = new httpRequest(GetURI(BookingEndpoint)).AddJsonBody(newBook);
        var postRestResponse = await httpClient.ExecutePostAsync<BookingInfoModel>(postRestRequest);

        //Verify POST request status code
        Assert.AreEqual(HttpStatusCode.OK, postRestResponse.StatusCode, "Status code is not equal to 200");
        #endregion

        #region GetBooking
        var restRequest = new RestRequest(GetURI($"{BookingEndpoint}/{newBook.Bookingdates}"), Method.Get);
        var restResponse = await restClient.ExecuteAsync<BookingInfoModel>(restRequest);
        #endregion

        #region Assertions
        Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode, "Status code is not equal to 200");
        Assert.AreEqual(newBook.Firstname, restResponse.Data.Firstname, "Firstname does not match.");
        Assert.AreEqual(newBook.Lastname, restResponse.Data.Lastname, "Lastname does not match.");
        Assert.AreEqual(newBook.Totalprice, restResponse.Data.Totalprice, "Total price does not match.");
        Assert.AreEqual(newBook.Depositpaid, restResponse.Data.Depositpaid, "Deposit paid does not match.");
        Assert.AreEqual(newBook.Bookingdates.Checkin, restResponse.Data.Bookingdates.Checkin, "Check in date does not match.");
        Assert.AreEqual(newBook.Bookingdates.Checkout, restResponse.Data.Bookingdates.Checkout, "Check out does not match.");
        Assert.AreEqual(newBook.Additionalneeds, restResponse.Data.Additionalneeds, "Additinal need does not match.")
            #endregion

        #region CleanUp
            cleanUpList.Add(newBook);
        #endregion
    }

    [TestMethod]
    public async Task UpdateBookingR()
    {
    }

    [TestMethod]
    public async Task DeleteBookingR()
    {
    }

    [TestMethod]
    public async Task InvalidBookingR()
    {
    }
}
}