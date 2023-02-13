using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class ProductAdd
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string OrderProductID { get; set; }
        public string ProductID { get; set; }
        //[ForeignKey("ProductID")]
        //public virtual Product Product { get; set; }
        [ForeignKey("OrderProductID")]
        public virtual OrderProduct OrderProduct { get; set; }
    }
}
