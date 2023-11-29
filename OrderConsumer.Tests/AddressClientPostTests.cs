using NUnit.Framework;
using OrderConsumer.Models;
using System.Net;

namespace OrderConsumer.Tests
{
    [TestFixture]
    public class AddressClientPostTests : PactTestBase
    {
        [Test]
        public async Task PostAddress_AddressIdIsValid()
        {
            Address address = new Address
            {
                Id = new Guid(addressIdNonexistent),
                AddressType = "delivery",
                Street = "Main Street",
                Number = 123,
                City = "Beverly Hills",
                ZipCode = 90210,
                State = "California",
                Country = "United States"
            };

            pact.UponReceiving("A request to create an address by ID")
                    .Given("an address with ID {id} does not exist", new Dictionary<string, string> { ["id"] = addressIdNonexistent })
                    .WithRequest(HttpMethod.Post, $"/address")
                    .WithBody(postPayload, "application/json; charset=utf-8")
                .WillRespond()
                    .WithStatus(HttpStatusCode.Created);
            
            await pact.VerifyAsync(async ctx => {
                var response = await client.PostAddress(address);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            });
        }
    }
}
