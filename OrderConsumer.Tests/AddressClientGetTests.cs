using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace OrderConsumer.Tests
{
    [TestFixture]
    public class AddressClientGetTests : PactTestBase
    {
        [Test]
        public async Task GetAddress_AddressIdExists()
        {
            pact.UponReceiving("A request to retrieve an address by ID")
                    .Given("an address with ID {id} exists", new Dictionary<string, string> { ["id"] = addressIdExisting })
                    .WithRequest(HttpMethod.Get, $"/address/{addressIdExisting}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithJsonBody(address);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress(addressIdExisting);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                string responseBodyAsString = await response.Content.ReadAsStringAsync();
                dynamic responseContent = JsonConvert.DeserializeObject<dynamic>(responseBodyAsString)!;
                Assert.That(responseContent.id.Value, Is.EqualTo(addressIdExisting));
            });
        }

        [Test]
        public async Task GetAddress_AddressIdDoesNotExist()
        {
            pact.UponReceiving("A request to retrieve an address by ID")
                    .Given("an address with ID {id} does not exist", new Dictionary<string, string> { ["id"] = addressIdNonexistent })
                    .WithRequest(HttpMethod.Get, $"/address/{addressIdNonexistent}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.NotFound);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress(addressIdNonexistent);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GetAddress_AddressIdIsInvalid()
        {
            pact.UponReceiving("A request to retrieve an address by ID")
                    .Given($"no specific state required")
                    .WithRequest(HttpMethod.Get, "/address/invalid_address_id")
                .WillRespond()
                    .WithStatus(HttpStatusCode.BadRequest);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress("invalid_address_id");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
    }
}
