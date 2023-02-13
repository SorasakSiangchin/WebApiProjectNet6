using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IStatusDeliveryService
    {
        Task<IEnumerable<StatusDelivery>> GetAll();
    }
}
