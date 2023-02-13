namespace BackEnd.DTOS.ProductDescription
{
    public class ProductDescriptionResponse
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string Image { get; set; }    
        static public ProductDescriptionResponse FromProductDescription(Models.ProductDescription productDescription)
        {
            // return ตัวมันเอง
            return new ProductDescriptionResponse
            {
              ID = productDescription.ID,
              ProductID = productDescription.ProductID, 
              Image = !string.IsNullOrEmpty(productDescription.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + productDescription.Image : "",
            };
        }
    }
}
