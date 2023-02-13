namespace BackEnd.DTOS.OrderCustomer
{
    public class OrderPaymentRequest
    {
        public string? ID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
    }
}
