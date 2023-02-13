using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class OrderCustomer
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public bool PaymentStatus { get; set; }
        public string? ProofOfPayment { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int PriceTotal { get; set; }
        public bool CustomerStatus { get; set; }
        public bool SellerStatus { get; set; }
        public string AddressID { get; set; }
        [ForeignKey("AddressID")]
        //[ValidateNever]
        public virtual Address Address { get; set; }
       
    }
}
