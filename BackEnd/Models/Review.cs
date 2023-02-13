using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string Data { get; set; } 
        public DateTime Created { get; set; } = DateTime.Now;
        public string? Video { get; set; }
        public int Score { get; set; }
        public string ScoreText { get; set; }
        public string CustomerID { get; set; }
        public string ProductListID { get; set; }
        [ForeignKey("ProductListID")]
        public virtual ProductList ProductList { get; set;  }
    }
}
