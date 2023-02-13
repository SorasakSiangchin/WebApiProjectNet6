using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class ReviewImageService : IReviewImageService
    {
        private readonly DatabaseContext databaseContext;

        public ReviewImageService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task Create(List<string> imageName, string ReviewID)
        {
            for(int i = 0; i < imageName.Count(); i++)
            {
                databaseContext.Add(new ReviewImage { ID = await GenerateID() , Image= imageName[i] , ReviewID = ReviewID });
                databaseContext.SaveChanges();
            }
            
        }
        public async Task<string> GenerateID()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.ReviewImages.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }

        public async Task<List<ReviewImage>> GetByIdReview(string idReview)
        {
            var result = await databaseContext.ReviewImages.Where(x => x.ReviewID == idReview).ToListAsync();
            if (result == null) return null;
            return result;  
        }
    }
}
