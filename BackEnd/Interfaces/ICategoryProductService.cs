using BackEnd.Models;

namespace BackEnd.interfaces
{
    public interface ICategoryProductService
    {
        Task<IEnumerable<CategoryProduct>> GetAll();
    }
}
