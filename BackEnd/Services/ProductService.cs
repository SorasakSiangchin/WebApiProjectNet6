using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;

        public ProductService(DatabaseContext databaseContext , IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
        }
        public async Task Create(Product product)
        {
            if (string.IsNullOrEmpty(product.ID)) product.ID = GenerateID();
            

            await databaseContext.Products.AddAsync(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            databaseContext.Remove(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAll(string searchName = "", string searchCategory = "") {
            var data = databaseContext.Products.Include(e => e.CategoryProduct).AsQueryable(); ;

            if (!string.IsNullOrEmpty(searchName)) data =  data.Where(a => a.Name.Contains(searchName));


            if (!string.IsNullOrEmpty(searchCategory)) data = data.Where(a => a.CategoryProduct.Name.Contains(searchCategory));
          
            
            return data;
        }

        public async Task<IEnumerable<Product>> GetByIdCustomer(string searchName = "", string searchCategory = "" , string idCustomer = "")
        {
            var data = databaseContext.Products.Include(e => e.CategoryProduct).AsQueryable(); 

            if (!string.IsNullOrEmpty(searchName)) data = data.Where(a => a.Name.Contains(searchName) && a.CustomerID.Equals(idCustomer));
            
            if (!string.IsNullOrEmpty(searchCategory)) data = data.Where(a => a.CategoryProduct.Name.Contains(searchCategory) && a.CustomerID.Equals(idCustomer));
           
            return data.Where(a => a.CustomerID.Equals(idCustomer));
        }


        public async Task<Product> GetByID(string ID) {
            var result = await databaseContext.Products.Include(e => e.CategoryProduct).SingleOrDefaultAsync(x => x.ID == ID);
            if (result == null)
            {
                throw new Exception("ไม่พบสินค้า");
            }
            return result;
        }



        public async Task<IEnumerable<Product>> Search(string name)
        {
           return await databaseContext.Products.Include(e => e.CategoryProduct).Where(e => e.Name.Contains(name)).ToListAsync();
        }

        public async Task Update(Product product)
        {
            databaseContext.Products.Update(product);
            await databaseContext.SaveChangesAsync();
        }


        //คือการสร้าง Code แบบ Auto
        public string GenerateID()
        {
            var result = databaseContext.Products.OrderByDescending(a => a.ID).FirstOrDefault();
            if (result != null)
            {
                int ID = Convert.ToInt32(result.ID);
                return (ID + 1).ToString();
            }
            return "1";
        }

        public async Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage = string.Empty;
            //var imageName = new List<string>();
            var imageName = string.Empty;
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await uploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMessage, imageName);
        }

        public async Task DeleteImage(string fileName)
        {
            await uploadFileService.DeleteImage(fileName);
        }

        public async Task<IEnumerable<Product>> GetNew(int num)
        {
            return await databaseContext.Products.Include(e => e.CategoryProduct).OrderByDescending(a => a.ID).Take(num).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetForSeller(int num = 0, string searchName = "", string searchCategory = "", string idCustomer = "")
        {
            var data = databaseContext.Products.Include(e => e.CategoryProduct).AsQueryable();

            if (!string.IsNullOrEmpty(searchName)) data = data.Where(a => a.Name.Contains(searchName) && a.CustomerID.Equals(idCustomer));

            if (!string.IsNullOrEmpty(searchCategory)) data = data.Where(a => a.CategoryProduct.Name.Contains(searchCategory) && a.CustomerID.Equals(idCustomer));

            return data.Where(a => a.CustomerID.Equals(idCustomer)).Take(num);
        }
    }
}
