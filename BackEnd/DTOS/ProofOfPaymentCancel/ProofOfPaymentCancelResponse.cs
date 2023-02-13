using BackEnd.DTOS.Product;
using BackEnd.Models;

namespace BackEnd.DTOS.ProofOfPaymentCancel
{
    public class ProofOfPaymentCancelResponse
    {
        public string ID { get; set; }
        public string OrderID { get; set; }
        public string ProofOfPayment { get; set; }
        public DateTime Created { get; set; }
        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public ProofOfPaymentCancelResponse FromProofOfPaymentCancel(Models.ProofOfPaymentCancel proofOfPaymentCancel)
        {
            // return ตัวมันเอง
            return new ProofOfPaymentCancelResponse
            {
               ID = proofOfPaymentCancel.ID,
               OrderID = proofOfPaymentCancel.OrderID,
                ProofOfPayment = !string.IsNullOrEmpty(proofOfPaymentCancel.ProofOfPayment) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + proofOfPaymentCancel.ProofOfPayment : "",
                Created = proofOfPaymentCancel.Created,

            };
        }
    }
}
