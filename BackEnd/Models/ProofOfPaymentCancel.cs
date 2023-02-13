using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class ProofOfPaymentCancel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string ProofOfPayment { get; set; }
        public DateTime Created { get; set; } 
        [ForeignKey("OrderID")]
        //[ValidateNever]
        public virtual OrderCustomer OrderCustomer { get; set; }
    }
}
