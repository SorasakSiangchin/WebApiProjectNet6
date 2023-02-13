using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IProofOfPaymentCancelService
    {
        Task<IEnumerable<ProofOfPaymentCancel>> GetByIdOrder(string idOrder);
    }
}
