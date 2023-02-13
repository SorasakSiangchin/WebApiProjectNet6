using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class CartCustomer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public int AmountProduct { get; set; } 
        public DateTime Created { get; set; } = DateTime.Now;
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

    }
}
