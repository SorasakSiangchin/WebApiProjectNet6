using BackEnd.DTOS.ProductDescription;
using BackEnd.interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductDescriptionController : ControllerBase
    {
        private readonly IProductDescriptionService productDescriptionService;

        public ProductDescriptionController(IProductDescriptionService productDescriptionService)
        {
            this.productDescriptionService = productDescriptionService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductDescriptionAll(string idProduct)
        {
           var result = (await productDescriptionService.GetAll(idProduct)).Select(ProductDescriptionResponse.FromProductDescription);
            if (result.Count() == 0) return Ok(new { mag = "ไม่พบข้อมูล" });
            return Ok(new { msg = "OK" , data= result });
        }

        [HttpPost("[action]")]
        // [FromBody] คือ Json 
        public async Task<ActionResult<ProductDescription>> AddProductDescription([FromForm] ProductDescriptionRequest productDescriptionRequest )
        {
            #region จัดการรูปภาพ
            (string erorrMesage, List<string> imageName) = await productDescriptionService.UploadImage(productDescriptionRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            #endregion

            var productDescription = productDescriptionRequest.Adapt<ProductDescription>();
            await productDescriptionService.Create(productDescription , imageName);

            return Ok(new {msg = "OK" });
        }
    }
}
