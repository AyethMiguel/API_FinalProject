using ServiceReference1;

namespace SOAPCountry
{
    [TestClass]
    public class CountryDetails
    {
        private ServiceReference1.CountryInfoServiceSoapTypeClient countryInfoServiceSoapType;

        [TestInitialize]
        public async Task Initialize()
        {
            countryInfoServiceSoapType = new ServiceReference1.CountryInfoServiceSoapTypeClient(ServiceReference1.CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        [TestMethod]
        public void CountryModel()
        {
            var country = ListOfCountryNamesByCode();
            var randomCountry = GetRandomRecord(country);

            var fullCountryInfo = countryInfoServiceSoapType.FullCountryInfo(randomCountry.sISOCode);

            Assert.AreEqual(fullCountryInfo.sISOCode, randomCountry.sISOCode);
            Assert.AreEqual(fullCountryInfo.sName, randomCountry.sName);
        }

        [TestMethod]
        public void CountryRandom()
        {
            var country = ListOfCountryNamesByCode();
            List<tCountryCodeAndName> countryRecords = new List<tCountryCodeAndName>();

            for (int record = 0; record < 5; record++)
            {
                countryRecords.Add(GetRandomRecord(country));
            }

            foreach (var countryRecord in countryRecords)
            {
                var isoCode = countryInfoServiceSoapType.CountryISOCode(countryRecord.sName);
                Assert.AreEqual(isoCode, countryRecord.sISOCode);
            }
        }

        private tCountryCodeAndName GetRandomRecord(List<tCountryCodeAndName> data)
        {
            var random = new Random();
            int next = random.Next(data.Count);

            var country = data[next];

            return country;
        }

        private List<tCountryCodeAndName> ListOfCountryNamesByCode()
        {
            var country = countryInfoServiceSoapType.ListOfCountryNamesByCode();

            return country;
        }
    }
}