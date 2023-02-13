namespace BackEnd.DTOS.Delivery
{
    public class DeliveryRequest
    {
        public string? ID { get; set; }
        public DateTime Date { get; set; }
        public string OrderCustomerID { get; set; }
        public int StatusDeliveryID { get; set; }
    }
}
