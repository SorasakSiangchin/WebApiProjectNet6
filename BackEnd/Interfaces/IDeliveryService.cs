using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IDeliveryService
    {
        Task<Delivery> GetByIdOrder(string idOrder);
        Task<IEnumerable<Delivery>> GetAll(string searchOrder , string idCustomer);
        Task<Delivery> GetById(string id);
        Task Add(Delivery delivery);
        Task Update(Delivery delivery);
    }
}
