using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    public class Delivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public DateTime? Date { get; set; }
        public string OrderCustomerID { get; set; }
        public int StatusDeliveryID { get; set; }
        [ForeignKey("OrderCustomerID")]
        public virtual OrderCustomer OrderCustomer { get; set; }
        [ForeignKey("StatusDeliveryID")]
        public virtual StatusDelivery StatusDelivery { get; set; }
    }
}
