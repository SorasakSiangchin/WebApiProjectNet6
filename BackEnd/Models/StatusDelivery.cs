using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class StatusDelivery
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
