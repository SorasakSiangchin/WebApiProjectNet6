using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
