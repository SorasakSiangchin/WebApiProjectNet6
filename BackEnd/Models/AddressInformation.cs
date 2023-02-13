using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class AddressInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public string? Detail { get; set; }
        public string CustomerName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhoneNumber { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        
        public string SubDistrict { get; set; }
        public string ZipCode { get; set; }



    }
}
