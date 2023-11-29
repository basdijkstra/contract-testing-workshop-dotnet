using AddressProvider.Tests.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PactNet.Verifier;

namespace AddressProvider.Tests
{
    [TestFixture]
    public class AddressPactTest
    {
        private const string PactServiceUri = "http://127.0.0.1:9876";

        [Test]
        public void VerifyThatAddressServiceHonoursPacts()
        {
            using var app = Startup.WebApp();
            app.Urls.Add(PactServiceUri);
            app.UseMiddleware<ProviderStateMiddleware>();
            app.Start();
            
            using var verifier = new PactVerifier(new PactVerifierConfig());
            var pactFolder = new DirectoryInfo(Path.Join("..", "..", "..", "pacts"));
            verifier.ServiceProvider("address_provider", new Uri(PactServiceUri))
                .WithPactBrokerSource(new Uri("https://ota.pactflow.io"), options =>
                {
                    options.TokenAuthentication("HbtH0tZq7CU4d18JlKR2kA").PublishResults("1.0.0");
                })
                .WithProviderStateUrl(new Uri($"{PactServiceUri}/provider-states"))
                .Verify();
        }
    }
}
