using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class ProductDescription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string Image { get; set; }
        public string ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

    }
}
