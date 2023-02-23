using AutoMapper;
using CityInfo.API.Model;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace CityInfo.API.Controller
{
    [ApiController]
    [Route("api/cities/{cityid}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }



        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if(!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when " +
                        $"accessing points of interest.");
                    return NotFound();
                }

                var pointsofInterestForCity = _cityInfoRepository.GetPointsOfInterestsForCity(cityId);
                
                var poicityResults=new List<PointOfInterestDto>();

                foreach(var pointOfInterest in pointsofInterestForCity)
                {
                    poicityResults.Add(new PointOfInterestDto()
                    {
                        Id = pointOfInterest.Id,
                        Name = pointOfInterest.Name,
                        Description = pointOfInterest.Description,
                    });
                }
                
                return Ok(poicityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for ciry with id {cityId}", ex);
                return StatusCode(500, "A Problem happened while handling your request.");
            }
            
        }

        [HttpGet("{id}",Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId,int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when " +
                    $"accessing points of interest.");
                return NotFound();
            }

            var pointOfInterest=_cityInfoRepository.GetPointOfInterestsForCity(cityId,id);
            
            if(pointOfInterest==null)
            {
                return NotFound();
            }

            var pointOfInterestResult=new PointOfInterestDto()
            {
                Id= pointOfInterest.Id,
                Name=pointOfInterest.Name,
                Description=pointOfInterest.Description,
            };

            return Ok(pointOfInterestResult);
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

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);
            _cityInfoRepository.Save();

            var createdPOIToReturn = _mapper.
                Map<Model.PointOfInterestDto>(finalPointOfInterest);
           

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = createdPOIToReturn.Id },
                createdPOIToReturn
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

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");

            return NoContent();
        }
    }
}
