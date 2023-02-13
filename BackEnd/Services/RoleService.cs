using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class RoleService : IRoleService
    {
        private readonly DatabaseContext databaseContext;

        public RoleService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await databaseContext.Roles.ToListAsync();
        }
    }
}
