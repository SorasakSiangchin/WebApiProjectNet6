namespace BackEnd.DTOS.ProductList
{
    public class ProductListResponse
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public int ProductPrice { get; set; }
        public int ProductAmount { get; set; }
        public string  ImageProduct { get; set; }
        public Models.Product Product { get; set; }
        public Models.OrderCustomer OrderCustomer { get; set; }
        static public ProductListResponse FromProductList (Models.ProductList productList)
        {
            // return ตัวมันเอง
            return new ProductListResponse
            {
                ID = productList.ID,
               OrderID = productList.OrderID,
               ProductID = productList.ProductID,
               ProductPrice = productList.ProductPrice,
               ProductAmount = productList.ProductAmount,
               Product = productList.Product,
                OrderCustomer = productList.OrderCustomer,
                ImageProduct = !string.IsNullOrEmpty(productList.Product.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + productList.Product.Image : "",
                
            };
        }
    }
}
