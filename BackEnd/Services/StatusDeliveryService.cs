using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class StatusDeliveryService : IStatusDeliveryService
    {
        private readonly DatabaseContext databaseContext;

        public StatusDeliveryService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<StatusDelivery>> GetAll()
        {
            var result = await databaseContext.StatusDeliveries.ToListAsync();
            return result;  
        }
    }
}
