using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }  
        [HttpGet("[action]")]
        public async Task<IEnumerable<Role>> GetRoleAll()
        {
            return await roleService.GetAll();
        }
    }
}
