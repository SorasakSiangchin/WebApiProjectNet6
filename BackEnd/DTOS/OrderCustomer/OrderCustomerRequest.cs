namespace BackEnd.DTOS.OrderCustomer
{
    public class OrderCustomerRequest
    {
        public string? ID { get; set; }
        public bool PaymentStatus { get; set; }
        public string? ProofOfPayment { get; set; }
        public int PriceTotal { get; set; }
        public bool? CustomerStatus { get; set; }
        public string AddressID { get; set; }
    }
}
