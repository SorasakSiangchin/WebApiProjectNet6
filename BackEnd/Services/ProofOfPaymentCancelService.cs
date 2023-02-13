using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class ProofOfPaymentCancelService : IProofOfPaymentCancelService
    {
        private readonly DatabaseContext databaseContext;

        public ProofOfPaymentCancelService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<ProofOfPaymentCancel>> GetByIdOrder(string idOrder)
        {
           return await databaseContext.ProofOfPaymentCancels.Where(e => e.OrderID == idOrder).ToListAsync();
        }
    }
}
