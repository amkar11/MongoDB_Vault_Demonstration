using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB_Vault_Demonstration.Application.ServiceInterfaces;

namespace MongoDB_Vault_Demonstration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("allcars")]
        public async Task<IActionResult> GetAllCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("singlecar")]
        public async Task<IActionResult> GetCarByIdAsync(string id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            return Ok(car);
        }

        [HttpPost("createcar")]
        public async Task<IActionResult> CreateCarAsync(string model)
        {
            await _carService.CreateCarAsync(model);
            return Created();
        }

        [HttpPost("deletecar")]
        public async Task<IActionResult> DeleteCarAsync(string id)
        {
            await _carService.DeleteCarByIdAsync(id);
            return Ok();
        }
    }
}
