using BackEnd.Models;

namespace BackEnd.interfaces
{
    public interface IProductDescriptionService
    {
        Task<IEnumerable<ProductDescription>> GetAll(string idProduct);
        Task<ProductDescription> GetByID(string ID);
        Task<(string errorMessage, List<string> imageName)> UploadImage(IFormFileCollection formFiles);
        Task Create(ProductDescription productDescription , List<string> imageName);
        Task DeleteImage(string fileName);
    }
}
