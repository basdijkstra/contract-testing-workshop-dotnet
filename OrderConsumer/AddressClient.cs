using OrderConsumer.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderConsumer
{
    public class AddressClient
    {
        private readonly Uri baseUri;

        public AddressClient(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> GetAddress(string id)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    var response = await client.GetAsync($"/address/{id}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the AddressProvider API.", ex);
                }
            }
        }

        public async Task<HttpResponseMessage> DeleteAddress(string id)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    var response = await client.DeleteAsync($"/address/{id}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the AddressProvider API.", ex);
                }
            }
        }

        public async Task<HttpResponseMessage> PostAddress(Address address)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    var response = await client.PostAsync($"/address",
                        new StringContent(JsonSerializer.Serialize(address), Encoding.UTF8, "application/json"));
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the AddressProvider API.", ex);
                }
            }
        }
    }
}