using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;

namespace CustomerConsumer.Tests
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

            pact = Pact.V3("customer_consumer", "address_provider", Config).WithHttpInteractions(port);
            client = new AddressClient(new Uri($"http://localhost:{port}"));

            /**
             * Define the expected structure for the address service GET response payload
             * 
             * The address should contain:
             * - A field Id matching the addressRegex defined above (use addressId as an example)
             *   HINT: Id = Match.<something>
             * - A field AddressType that should be a string (use any string value as an example)
             * - A field Street that should be a string
             * - A field Number that should be an integer
             * - A field City that should be a string
             * - A field ZipCode that should be a string
             * - A field State that should be a string
             * - A field Country that should be either 'United States' or 'Canada'
             */

            address = new
            {
                // The definition of the payload shape goes here
            };
        }
    }
}
