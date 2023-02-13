namespace BackEnd.DTOS.Product
{
    public class ProductResponse
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public DateTime Created { get; set; }
        public string CategoryName { get; set; }
        public int Weight { get; set; }
        public string DataMore { get; set; }
        public string Size { get; set; }
        public string CategoryID { get; set; }
        public string CustomerID { get; set; }
        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public ProductResponse FromProduct(Models.Product product)
        {
            // return ตัวมันเอง
            return new ProductResponse
            {
                ID = product.ID,
                Name = product.Name,
                Image = !string.IsNullOrEmpty(product.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + product.Image : "",
                Stock = product.Stock,
                Price = product.Price,
                CategoryName = product.CategoryProduct.Name,
                Created = product.Created,
                Color = product.Color,
                Weight = product.Weight,
                DataMore = product.DataMore,    
                Size = product.Size,    
                CategoryID = product.CategoryProductID,
                CustomerID = product.CustomerID,
            };
        }
    }
}
