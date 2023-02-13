using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.Product
{
    public class ProductRequest
    {
        public string? ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int Weight { get; set; }
        public string? DataMore { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        [Required]
        public string CategoryProductID { get; set; }
        [Required]
        public string CustomerID { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
    }
}
