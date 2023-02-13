namespace BackEnd.DTOS.CartCustomer
{
    public class CartCustomerResponse
    {
        public string ID { get; set; }
        //public Models.Customer Customer { get; set; }
        public Models.Product Product { get; set; }
        public int AmountProduct { get; set; }
        public string ImageProduct { get; set; }
        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public CartCustomerResponse FromCartCustomer(Models.CartCustomer cartCustomer) {
            // return ตัวมันเอง
            return new CartCustomerResponse
            {
                ID = cartCustomer.ID,
                AmountProduct = cartCustomer.AmountProduct,
                Product = cartCustomer.Product,
                //https://localhost:7280/
                ImageProduct = !string.IsNullOrEmpty(cartCustomer.Product.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + cartCustomer.Product.Image : "",
                //Customer = cartCustomer.Customer
            };
        }
    }
}
