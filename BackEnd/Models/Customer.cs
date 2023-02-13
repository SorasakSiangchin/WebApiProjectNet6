using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[StringLength(15, MinimumLength = 3)]
        public string ID { get; set; }
        public string Name { get; set; }
        //[EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string? Image { get; set; }
        public int? RoleID { get; set; }
        [ForeignKey("RoleID")]
        //[ValidateNever]
        public virtual Role Role { get; set; }
    }
}
