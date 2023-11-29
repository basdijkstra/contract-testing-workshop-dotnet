using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;

namespace OrderConsumer.Tests
{
    public class PactTestBase
    {
        protected IPactBuilderV3 pact;
        protected AddressClient client;
        protected object address;
        protected readonly string addressIdExisting = "93edc1a1-5093-4d30-a9c1-da04765553b7";
        protected readonly string addressIdNonexistent = "3514466e-3e58-48b3-ab35-e553b91aa2b3";
        private readonly string addressRegex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        protected readonly string postPayload = "{\"Id\":\"3514466e-3e58-48b3-ab35-e553b91aa2b3\",\"AddressType\":\"delivery\",\"Street\":\"Main Street\",\"Number\":123,\"City\":\"Beverly Hills\",\"ZipCode\":90210,\"State\":\"California\",\"Country\":\"United States\"}";

        private string pactDir = Path.Join("..", "..", "..", "pacts");
        private readonly int port = 9876;

        [SetUp]
        public void SetUp()
        {
            var Config = new PactConfig
            {
                PactDir = pactDir,
                DefaultJsonSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            pact = Pact.V3("order_consumer", "address_provider", Config).WithHttpInteractions(port);
            client = new AddressClient(new Uri($"http://localhost:{port}"));

            address = new
            {
                Id = Match.Regex(addressIdExisting, addressRegex),
                AddressType = Match.Type("billing"),
                Street = Match.Type("Main street"),
                Number = Match.Integer(123),
                City = Match.Type("Los Angeles"),
                ZipCode = Match.Integer(12345),
                State = Match.Type("California"),
                Country = Match.Regex("United States", "United States|Canada")
            };
        }
    }
}
