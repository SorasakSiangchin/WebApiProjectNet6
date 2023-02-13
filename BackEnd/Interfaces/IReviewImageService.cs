using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IReviewImageService
    {
        Task<List<ReviewImage>> GetByIdReview(string idReview);
        Task Create(List<string> imageName , string ReviewID);
    }
}
