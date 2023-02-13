using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class ReviewImage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string Image { get; set; }
        public string ReviewID { get; set; }
        [ForeignKey("ReviewID")]
        public virtual Review Review { get; set; }
    }
}
