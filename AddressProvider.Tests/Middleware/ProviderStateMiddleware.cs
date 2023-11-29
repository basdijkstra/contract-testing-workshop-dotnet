using AddressProvider.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace AddressProvider.Tests.Middleware
{
    public class ProviderStateMiddleware
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Func<IDictionary<string, object>, Task>> _providerStates;
        private readonly AddressDatabase _addressDatabase;

        public ProviderStateMiddleware(RequestDelegate next, AddressDatabase addressDatabase)
        {
            _next = next;
            _addressDatabase = addressDatabase;
            _providerStates = new Dictionary<string, Func<IDictionary<string, object>, Task>>
            {
                ["an address with ID {id} exists"] = CreateAddress,
                ["an address with ID {id} does not exist"] = DeleteAddress,
                ["no specific state required"] = DoNothing
            };
        }

        private async Task DoNothing(IDictionary<string, object> parameters)
        {
        }

        private async Task CreateAddress(IDictionary<string, object> parameters)
        {
            // In 'real life', we would explicitly add an address to the database here.
        }

        private async Task DeleteAddress(IDictionary<string, object> parameters)
        {
            // In 'real life', we would explicitly delete an address from the database here.
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false))
            {
                await this._next.Invoke(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status200OK;

            if (context.Request.Method == HttpMethod.Post.ToString())
            {
                string jsonRequestBody;

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                try
                {
                    ProviderState providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, Options);

                    if (!string.IsNullOrEmpty(providerState?.State))
                    {
                        await this._providerStates[providerState.State].Invoke(providerState.Params);
                    }
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Failed to deserialise JSON provider state body:");
                    await context.Response.WriteAsync(jsonRequestBody);
                    await context.Response.WriteAsync(string.Empty);
                    await context.Response.WriteAsync(e.ToString());
                }
            }
        }
    }
}
