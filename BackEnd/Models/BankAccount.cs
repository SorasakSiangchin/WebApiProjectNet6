using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class BankAccount
    {
        [Key]
        public int ID { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string BankNumber { get; set; }
        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}
