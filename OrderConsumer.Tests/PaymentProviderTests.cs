using NUnit.Framework;
using OrderConsumer.Models;
using System.Net;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace OrderConsumer.Tests
{
    [TestFixture]
    public class PaymentProviderTests
    {
        protected WireMockServer Server { get; private set; }

        protected static readonly int MOCK_SERVER_PORT = 9876;

        protected static readonly string MOCK_SERVER_BASE_URL = $"http://localhost:{MOCK_SERVER_PORT}";

        [OneTimeSetUp]
        public void StartServer()
        {
            this.Server = WireMockServer.Start(MOCK_SERVER_PORT);
        }

        [Test]
        public async Task GetPayment_ValidExistingOrderId_ShouldYieldHttp200()
        {
            this.SetupStubValidExistingPaymentId();

            var response = await new PaymentClient(new Uri(this.Server.Url))
                .GetPayment("8383a7c3-f831-4f4d-a0a9-015165148af5");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetPayment_ValidNonExistingOrderId_ShouldYieldHttp404()
        {
            this.SetupStubValidNonExistingPaymentId();

            var response = await new PaymentClient(new Uri(this.Server.Url))
                .GetPayment("00000000-0000-0000-0000-000000000000");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetPayment_InvalidOrderId_ShouldYieldHttp400()
        {
            this.SetupStubInvalidPaymentId();

            var response = await new PaymentClient(new Uri(this.Server.Url))
                .GetPayment("invalid_payment_id");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private void SetupStubValidExistingPaymentId()
        {
            Payment payment = new Payment()
            {
                Id = Guid.Parse("8383a7c3-f831-4f4d-a0a9-015165148af5"),
                OrderId = Guid.Parse("228aa55c-393c-411b-9410-4a995480e78e"),
                Status = "payment_complete",
                Amount = 42,
                Description = $"Payment for order 228aa55c-393c-411b-9410-4a995480e78e"
            };

            this.Server?
                .WithConsumer("order_consumer")
                .WithProvider("payment_provider")
                .Given(Request.Create().WithPath("/payment/8383a7c3-f831-4f4d-a0a9-015165148af5").UsingGet())
                .WithTitle("A GET request with a valid existing payment ID")
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(payment));
        }

        private void SetupStubValidNonExistingPaymentId()
        {
            this.Server?
                .WithConsumer("order_consumer")
                .WithProvider("payment_provider")
                .Given(Request.Create().WithPath("/payment/00000000-0000-0000-0000-000000000000").UsingGet())
                .WithTitle("A GET request with a valid nonexisting payment ID")
                .RespondWith(Response.Create()
                .WithStatusCode(404));
        }

        private void SetupStubInvalidPaymentId()
        {
            this.Server?
                .WithConsumer("order_consumer")
                .WithProvider("payment_provider")
                .Given(Request.Create().WithPath("/payment/invalid_payment_id").UsingGet())
                .WithTitle("A GET request with an invalid payment ID")
                .RespondWith(Response.Create()
                .WithStatusCode(400));
        }

        [OneTimeTearDown]
        protected void StopServer()
        {
            this.Server.SavePact(Path.Combine("../../../", "pacts"), "order_consumer-payment_provider.json");

            this.Server?.Stop();
        }
    }
}
