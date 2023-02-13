using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class ProductDescriptionService : IProductDescriptionService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;

        public ProductDescriptionService(DatabaseContext databaseContext, IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
        }

        public async Task Create(ProductDescription productDescription , List<string> imageName)
        {
            for (var i = 0; i < imageName.Count(); i++)
            {
                productDescription.ID = await GenerateIdProductDescription();
                productDescription.Image = imageName[i];
                await databaseContext.ProductDescriptions.AddAsync(productDescription);
                await databaseContext.SaveChangesAsync();
            }
        }

        public Task DeleteImage(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDescription>> GetAll(string idProduct)
        {
           return  await databaseContext.ProductDescriptions.Where(e => e.ProductID.Equals(idProduct)).ToListAsync();
;           
        }

        public async Task<ProductDescription> GetByID(string ID)
        {
            var result = await databaseContext.ProductDescriptions.AsNoTracking().FirstOrDefaultAsync(e => e.ID == ID);
            return result;
        }

        public async Task<(string errorMessage, List<string> imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage = string.Empty;
            var imageName = new List<string>();
            //var imageName = string.Empty;
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await uploadFileService.UploadImages(formFiles));
                }
            }
            return (errorMessage, imageName);
        }

        public async Task<string> GenerateIdProductDescription()
        {
            Random randomNumber = new Random();
            string IdProductDescription = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductDescription = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.ProductDescriptions.FindAsync(IdProductDescription);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductDescription;
        }
    }
}
