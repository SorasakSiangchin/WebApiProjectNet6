using BackEnd.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressInformationController : ControllerBase
    {
        private readonly IAddressInformationService addressInformationService;

        public AddressInformationController(IAddressInformationService addressInformationService)
        {
            this.addressInformationService = addressInformationService;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAddressInformationById(string id)
        {
            var result = await addressInformationService.GetById(id);
            return Ok(result);
        }
    }
}
