namespace OrderConsumer.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status {  get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
