using BackEnd.Models;

namespace BackEnd.interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll(string searchName = "", int searchRole = 0);
        Task<Customer> GetByID(string ID);
        Task<object> Register(Customer customer);

        Task<Customer> GetShopKeeperByID(string ID);
        Task Update(Customer customer);
        Task<Customer> Login(string email, string password);
        // ถ้า login เสร็จแล้ว ก็ส่งตั๋วไปให้   
        string GenerateToken(Customer customer);
        // เอาไว้แสดงข้อมูลใน Token
        Customer GetInfo(string customerToken);
        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);
        Task DeleteImage(string fileName);
    }
}
