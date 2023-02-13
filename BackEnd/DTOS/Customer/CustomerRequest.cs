using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.Customer
{
    public class CustomerRequest
    {

       
        public string ID { get; set; }
      
        public string? Name { get; set; }
       
        //[EmailAddress]
        public string? Email { get; set; }
       
        public string? Password { get; set; }
        //[DataType(DataType.PhoneNumber)]
      
        [Phone]
        public string? PhoneNumber { get; set; }
    
        public DateTime Birthday { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
        public int? RoleID { get; set; }
    }
}
