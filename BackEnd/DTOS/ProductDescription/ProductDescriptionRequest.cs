namespace BackEnd.DTOS.ProductDescription
{
    public class ProductDescriptionRequest
    {
        public string? ID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
        public string ProductID { get; set; }
    }
}
