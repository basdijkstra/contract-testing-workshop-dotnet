using OrderConsumer.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderConsumer
{
    public class PaymentClient
    {
        private readonly Uri baseUri;

        public PaymentClient(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> GetPayment(string id)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    var response = await client.GetAsync($"/payment/{id}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the PaymentProvider API.", ex);
                }
            }
        }
    }
}