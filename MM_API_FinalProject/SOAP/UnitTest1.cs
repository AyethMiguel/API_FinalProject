using ServiceReference1;

namespace SOAP
{
    [TestClass]
    public class UnitTest1
    {
        // Global Service
        private readonly CountryInfoServiceSoapTypeClient countryInfoService =
           new(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

        [TestMethod]
        public void ListModel()
        {
            var countryList = countryInfoService.ListOfCountryNamesByCode();
        }

        [TestMethod]
        public async ListString()
        {
        }
    }
}