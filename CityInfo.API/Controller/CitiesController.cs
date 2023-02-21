using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.current.Cities);
        }

        [HttpGet("{id}")]
        public JsonResult GetCity(int id)
        {
            return new JsonResult(CitiesDataStore.current.Cities.FirstOrDefault(x=>x.Id== id));
        }

    }
}
