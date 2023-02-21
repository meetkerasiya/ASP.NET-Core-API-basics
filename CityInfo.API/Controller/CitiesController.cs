using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCities()
        {

            return Ok(CitiesDataStore.current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToReturn=CitiesDataStore.current.Cities.FirstOrDefault(x=>x.Id==id);
            if(cityToReturn==null)
            {
                return NotFound();
            }    

            return Ok(cityToReturn);
           
        }

    }
}
