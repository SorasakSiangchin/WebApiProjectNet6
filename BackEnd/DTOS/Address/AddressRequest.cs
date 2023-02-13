using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.Address
{
    public class AddressRequest
    {
       
        [Required]
        public string CustomerID { get; set; }
        public string? AddressInformationID { get; set; }
        public int? StatusAddressID { get; set; }
    }
}
