using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class SellerMustConfrim
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public int BankAccountID { get; set; }
        public string ProductListID { get; set; }
        public string OrderID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [ForeignKey("BankAccountID")]
        public virtual BankAccount BankAccount { get; set; }
    }
}
