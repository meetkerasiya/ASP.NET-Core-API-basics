using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller
{
    [ApiController]
    [Route("api/cities/{cityid}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city=CitiesDataStore.current.Cities.FirstOrDefault(x=>x.Id==cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}")]
        public IActionResult GetPointOfInterest(int cityId,int id)
        {
            var city=CitiesDataStore.current.Cities.FirstOrDefault(x=>x.Id==cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest=city.PointsOfInterest.FirstOrDefault(x=>x.Id==id);

            if(pointOfInterest==null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }


    }
}
