using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public int StatusAddressID { get; set; }
  
        public string AddressInformationID { get; set; }
        [ForeignKey("AddressInformationID")]
        //[ValidateNever]
        public virtual AddressInformation AddressInformation { get; set; }
        [ForeignKey("CustomerID")]
        //[ValidateNever]
        public virtual Customer Customer { get; set; }
        [ForeignKey("StatusAddressID")]
        public virtual StatusAddress StatusAddress { get; set; }
    }
}
