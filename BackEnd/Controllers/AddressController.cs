using BackEnd.DTOS.Address;
using BackEnd.Interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }
       
        [HttpGet("[action]/{idCustomer}")]
        public async Task<IActionResult> GetAddressAll(string idCustomer)
        {
            var result = (await addressService.GetAll(idCustomer)).Select(AddressResponse.FromAddress);
          
           return  Ok(result);
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAddress([FromForm] AddressInformationRequest addressInformationRequest , [FromForm] AddressRequest address)
        {
            var addressInformation = addressInformationRequest.Adapt<AddressInformation>();
            var resultAddressInformationID = await addressService.CreateAddressInformation(addressInformation);
            var addresses = address.Adapt<Address>();
            addresses.AddressInformationID = resultAddressInformationID;
            await addressService.CreateAddress(addresses);
            return Ok(new {msg = "OK"});
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditAddressStatus( [FromForm] AddressRequest2 addressRequest2)
        {
            var addresses = addressRequest2.Adapt<Address>();
            await addressService.EditAddressStatus(addresses);
            return Ok(new { msg = "OK" });
        }
    }
}
