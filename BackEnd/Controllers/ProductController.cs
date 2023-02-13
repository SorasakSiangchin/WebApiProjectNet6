using BackEnd.DTOS.Product;
using BackEnd.interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public ProductController(IProductService productService , ICustomerService customerService)
        {
            this.productService = productService;
            this.customerService = customerService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductAll(string? searchName = "" , string? searchCategory = "")
        {
            var result =( await productService.GetAll(searchName , searchCategory)).Select(ProductResponse.FromProduct);
            if (result.Count() == 0) return Ok(new { msg = "ไม่พบสินค้า" });
            return Ok(new { msg = "OK", data = result });
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductForSeller(int num = 0, string? searchName = "", string? searchCategory = "", string idCustomer = "")
        {
            var result = (await productService.GetForSeller(num, searchName, searchCategory, idCustomer)).Select(ProductResponse.FromProduct);
            if (result.Count() == 0) return Ok(new { msg = "ไม่พบสินค้า" });
            return Ok(new { msg = "OK", data = result });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductNew(int num)
        {
            var result = (await productService.GetNew(num)).Select(ProductResponse.FromProduct);
            if (result.Count() == 0) return Ok(new { msg = "ไม่พบสินค้า" });
            return Ok(new { msg = "OK", data = result });
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductByID(string id)
        {
            return Ok(ProductResponse.FromProduct(await productService.GetByID(id)));
        }

        [HttpGet("[action]/{idCustomer}")]
        public async Task<IActionResult> GetProductByIdCustomer(string? searchName = "", string? searchCategory = "" ,string idCustomer = "")
        {
            var result = (await productService.GetByIdCustomer(searchName, searchCategory,idCustomer)).Select(ProductResponse.FromProduct);
            if (result.Count() == 0) return Ok(new {msg = "ไม่พบสินค้า"});
            return Ok(new { msg = "OK" , data= result });
        }

        //[Authorize(Roles = "Admin,Seller")]
        [HttpPost("[action]")]
        // [FromBody] คือ Json 
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {
            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            #endregion
    
            var result = await customerService.GetByID(productRequest.CustomerID);
            if (result == null) return Ok(new { msg = "ไม่พบผู้ใช้งาน" });
            
            var product = productRequest.Adapt<Product>();
            product.Image = imageName;
            await productService.Create(product);

            return Ok(new {msg = "OK" , data = product });
        }

        //[Authorize(Roles = "Admin,Seller")]
        [HttpPut("[action]")]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] ProductRequest productRequest)
        {
            var product = await productService.GetByID(productRequest.ID);
            if (product == null) return Ok(new { msg = "ไม่พบสินค้า" });
            
            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            if (!string.IsNullOrEmpty(imageName))
            {
                await productService.DeleteImage(product.Image);
                product.Image = imageName;
            }
            #endregion
            var result = productRequest.Adapt(product);
            if (productRequest.FormFiles == null) result.Image = product.Image;
            await productService.Update(result);
            return Ok(new { msg = "OK" , data = result });
        }
        //[Authorize(Roles = "Admin,Seller")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await productService.GetByID(id);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }

            await productService.Delete(result);
            await productService.DeleteImage(result.Image);
            return Ok(new { msg = "OK" , data = result });
        }
    }
}
