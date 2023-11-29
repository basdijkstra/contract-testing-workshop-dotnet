using NUnit.Framework;
using System.Net;

namespace CustomerConsumer.Tests
{
    [TestFixture]
    public class AddressClientDeleteTests : PactTestBase
    {
        [Test]
        public async Task DeleteAddress_AddressIdIsValid()
        {
            pact.UponReceiving("A request to delete an address by ID")
                    .Given("an address with ID {id} exists", new Dictionary<string, string> { ["id"] = addressIdExisting })
                    .WithRequest(HttpMethod.Delete, $"/address/{addressIdExisting}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.NoContent);

            await pact.VerifyAsync(async ctx => {
                var response = await client.DeleteAddress(addressIdExisting);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public async Task DeleteAddress_AddressIdIsInvalid()
        {
            pact.UponReceiving("A request to delete an address by ID")
                    .Given($"no specific state required")
                    .WithRequest(HttpMethod.Delete, "/address/invalid_address_id")
                .WillRespond()
                    .WithStatus(HttpStatusCode.BadRequest);

            await pact.VerifyAsync(async ctx => {
                var response = await client.DeleteAddress("invalid_address_id");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
    }
}
