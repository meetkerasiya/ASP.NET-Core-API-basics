using CityInfo.API.Model;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id}",Name ="GetPointOfInterest")]
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


        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, 
          [FromBody]  PointOfInterestForCreationDto pointOfInterest)
        {
            if(pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city=CitiesDataStore.current.Cities.FirstOrDefault(c=>c.Id==cityId);
            if(city== null)
            {
                return NotFound();
            }

            var maxPointOfInterestId= CitiesDataStore.current.Cities.SelectMany(
                x=>x.PointsOfInterest).Max(p=>p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,

            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = finalPointOfInterest.Id },
                finalPointOfInterest
                );
        }


        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId,int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == id);
            if(pointOfInterestFromStore==null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name= pointOfInterest.Name;
            pointOfInterestFromStore.Description= pointOfInterest.Description;

            return NoContent();
        }


        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId,int id,
            [FromBody] JsonPatchDocument<PointOfInterestDto> patchDoc)
        {
            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description,
            };

            patchDoc.ApplyTo(pointOfInterestToPatch,ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }
            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestFromStore.Name=pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description= pointOfInterestToPatch.Description;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId,int id)
        {
            var city = CitiesDataStore.current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }
    }
}
