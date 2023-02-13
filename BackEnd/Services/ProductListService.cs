using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class ProductListService : IProductListService
    {
        private readonly DatabaseContext databaseContext;
        public ProductListService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<ProductList>> GetAll(string idOrder)
        {
            return await databaseContext.ProductLists.Include(e => e.Product).Where(e => e.OrderID == idOrder).ToListAsync();
        }

        public async Task<List<ProductList>> GetByIdProduct(string idProduct)
        {
            return await databaseContext.ProductLists.Where(e => e.ProductID == idProduct).ToListAsync();    
        }

        public async Task<ProductList> GetById(string id)
        {
           var result = await databaseContext.ProductLists.Include(e => e.Product).SingleOrDefaultAsync(e => e.ID == id);
            return result;
        }

        public async Task<IEnumerable<ProductList>> GetProductOrdered(string idOrder, string idCustomer)
        {
            var data = await databaseContext.ProductLists.Include(e => e.Product).Where(e => e.OrderID == idOrder).ToListAsync();
            for (var i = 0;i < data.Count(); i++)
            {

            }
            return null;
        }

    }
}
