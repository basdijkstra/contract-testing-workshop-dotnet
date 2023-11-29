namespace AddressProvider.Tests.Middleware
{
    public class ProviderState
    {
        public string State { get; set; }
        public IDictionary<string, object> Params { get; set; }
    }
}
