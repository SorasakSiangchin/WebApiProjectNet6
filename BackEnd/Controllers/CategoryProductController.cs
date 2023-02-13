using BackEnd.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
        private readonly ICategoryProductService categoryProductService;

        public CategoryProductController(ICategoryProductService categoryProductService)
        {
            this.categoryProductService = categoryProductService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategoryProductAll()
        {
           return Ok(await categoryProductService.GetAll());
        }
    }
}
