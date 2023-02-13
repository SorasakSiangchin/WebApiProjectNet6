using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.Customer
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //[DataType(DataType.PhoneNumber)]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public IFormFileCollection? FormFiles { get; set; }
        public int RoleID { get; set; }
    }
}
