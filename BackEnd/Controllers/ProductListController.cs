using BackEnd.DTOS.ProductList;
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductListController : ControllerBase
    {
        private readonly IProductListService productListService;

        public ProductListController(IProductListService productListService)
        {
            this.productListService = productListService;
        }
        [HttpGet("[action]/{idOrder}")]
        public async Task<IActionResult> GetAll (string idOrder)
        {
            var data = (await productListService.GetAll(idOrder)).Select(ProductListResponse.FromProductList);
            return Ok(data);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = ProductListResponse.FromProductList(await productListService.GetById(id));
            if(data == null)
            {
                return Ok(new { msg = "ไม่พบข้อมูล" });
            }
            return Ok(new { msg = "OK" , data=data});
        }



    }
}
