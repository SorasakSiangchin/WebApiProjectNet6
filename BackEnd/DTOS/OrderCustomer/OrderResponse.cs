namespace BackEnd.DTOS.OrderCustomer
{
    public class OrderResponse
    {
            public string ID { get; set; }
            public bool PaymentStatus { get; set; }
            public string? ProofOfPayment { get; set; }
            public DateTime Created { get; set; } = DateTime.Now;
            public int PriceTotal { get; set; }
            public bool CustomerStatus { get; set; }
            public string AddressID { get; set; }
            public Models.Address Address { get; set; }
            public bool SellerStatus { get; set; }
        static public OrderResponse FromOrder(Models.OrderCustomer orderCustomer)
            {
                // return ตัวมันเอง
                return new OrderResponse
                {
                   ID = orderCustomer.ID,
                   PaymentStatus = orderCustomer.PaymentStatus,
                    ProofOfPayment = !string.IsNullOrEmpty(orderCustomer.ProofOfPayment) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + orderCustomer.ProofOfPayment : "",
                    PriceTotal = orderCustomer.PriceTotal,
                    CustomerStatus = orderCustomer.CustomerStatus,
                    AddressID = orderCustomer.AddressID,
                    Address = orderCustomer.Address,    
                    SellerStatus = orderCustomer.SellerStatus,
                };
            }
    }
}
