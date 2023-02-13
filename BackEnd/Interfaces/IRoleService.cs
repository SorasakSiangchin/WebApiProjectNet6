using BackEnd.Models;

namespace BackEnd.interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAll ();
    }
}
