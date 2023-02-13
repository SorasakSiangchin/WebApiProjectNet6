using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOS.CartCustomer
{
    public class CartCustomerRequest 
    {

        public string? ID { get; set; }
        [Required]
        public string CustomerID { get; set; }
        [Required]
        public string ProductID { get; set; }
        [Required]
        public int AmountProduct { get; set; }
    }
}
