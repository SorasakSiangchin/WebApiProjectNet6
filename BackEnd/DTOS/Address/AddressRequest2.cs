using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.Address
{
    public class AddressRequest2
    {
        public string ID { get; set; }
        [Required]
        public string CustomerID { get; set; }
        public string? AddressInformationID { get; set; }
        public int? StatusAddressID { get; set; }
    }
}
