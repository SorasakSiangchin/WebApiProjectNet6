namespace BackEnd.DTOS.OrderCustomer
{
    public class ProductListOrderRequest
    {
        public string[] CartID { get; set; }
        public string? OrderID { get; set; }
        public string[] ProductID { get; set; }
        public int[] ProductPrice { get; set; }
        public int[] ProductAmount { get; set; }
    }
}
