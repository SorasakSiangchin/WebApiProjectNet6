using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface ICartCustomerService
    {
        Task<IEnumerable<CartCustomer>> GetAll(string idCustomer);
        Task<CartCustomer> GetByID(string ID);
        Task Create(CartCustomer cartCustomer);
        Task Update(CartCustomer cartCustomer);
        Task Delete(CartCustomer cartCustomer);
        Task DeleteImage(string fileName);
    }
}
