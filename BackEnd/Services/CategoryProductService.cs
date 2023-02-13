using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class CategoryProductService : ICategoryProductService
    {
        private readonly DatabaseContext databaseContext;

        public CategoryProductService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<CategoryProduct>> GetAll()
        {
           return await databaseContext.CategoryProducts.ToListAsync();
        }


    }
}
