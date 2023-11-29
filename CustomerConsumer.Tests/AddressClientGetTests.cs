using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace CustomerConsumer.Tests
{
    [TestFixture]
    public class AddressClientGetTests : PactTestBase
    {
        [Test]
        public async Task GetAddress_AddressIdExists()
        {
            /**
             * Define the expected provider behaviour for the situation where
             * the address ID is valid and exists in the provider database
             * We should expect an HTTP 200 and a response body in the shape we defined just now
             *  
             * Have a look at the AddressClientDeleteTests.cs for inspiration
             * If you're really stuck, the same tests for the OrderConsumer contain an answer
             */
        }

        [Test]
        public async Task GetAddress_AddressIdDoesNotExist()
        {
            /**
             * Define the expected provider behaviour for the situation where
             * the address ID is valid but does not exist in the provider database
             * We should expect an HTTP 404, without further expectations about the response body
             * 
             * Have a look at the AddressClientDeleteTests.cs for inspiration
             * If you're really stuck, the same tests for the OrderConsumer contain an answer
             */            
        }

        [Test]
        public async Task GetAddress_AddressIdIsInvalid()
        {
            /**
             * Define the expected provider behaviour for the situation where
             * the address ID is invalid
             * We should expect an HTTP 400, without further expectations about the response body
             * 
             * Have a look at the AddressClientDeleteTests.cs for inspiration
             * If you're really stuck, the same tests for the OrderConsumer contain an answer
             */
        }
    }
}
