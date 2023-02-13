using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Weight { get; set; }
        public string? DataMore { get; set; }
        public string? Size { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string CustomerID { get; set; }
        public string CategoryProductID { get; set; }
        [ForeignKey("CategoryProductID")]
        public virtual CategoryProduct CategoryProduct { get; set; }


    }
}
