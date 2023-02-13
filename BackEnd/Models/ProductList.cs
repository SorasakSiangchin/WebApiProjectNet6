using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class ProductList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public int ProductPrice { get; set; }
        public int ProductAmount { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        [ForeignKey("OrderID")]
        public virtual OrderCustomer OrderCustomer { get; set; }
    }
}
