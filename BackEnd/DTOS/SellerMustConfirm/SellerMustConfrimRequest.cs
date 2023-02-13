using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.SellerMustConfirm
{
    public class SellerMustConfrimRequest
    {
        [Required]
        public string[] ID { get; set; }
    }
}
