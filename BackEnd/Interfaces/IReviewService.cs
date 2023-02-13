using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAll(string idProduct);
        Task<IEnumerable<Review>> GetByIdCustomer(string idCustomer , string idProduct);
        Task<Review> Create(Review review);
        Task<IEnumerable<Review>> GetByIdProductList(string idProductList);
        Task<(string erorrImage, List<string> imageName)> UploadImage(IFormFileCollection formFiles);
        Task<(string erorrVedio, string vedioName)> UploadVedio(IFormFileCollection formFiles);
        Task DeleteImage(string fileName);

    }
}
