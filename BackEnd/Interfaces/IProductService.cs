using BackEnd.Models;

namespace BackEnd.interfaces
{
    public interface IProductService 
    {
        Task<IEnumerable<Product>> GetAll(string searchName = "", string searchCategory = "");
        Task<IEnumerable<Product>> GetForSeller(int num = 0, string searchName = "", string searchCategory = "", string idCustomer = "");
        Task<IEnumerable<Product>> GetByIdCustomer(string searchName = "", string searchCategory = "", string idCustomer = "");
        Task<IEnumerable<Product>> GetNew(int num);
        Task<Product> GetByID (string ID);
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(Product product);
        Task<IEnumerable<Product>> Search(string name);
        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);
        Task DeleteImage(string fileName);
    }
}
