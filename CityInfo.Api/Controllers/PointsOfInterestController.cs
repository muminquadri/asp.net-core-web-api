using AutoMapper;
using CityInfo.Api.Interface;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private ICityInfoRepository _cityInfoRepository;
        // private LocalMailService _mailService;
        // we have 2 implementations if IMailService interface in startup.cs. either 1 is called depending on debug or release
        private IMailService _mailService;
        //public PointsOfInterestController(ILogger<PointsOfInterestController> logger, LocalMailService mailService)

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
        ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            //HttpContext.RequestServices.GetService();
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                 _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                var pointOfInterestForCityResults = Mapper.Map<IEnumerable<Models.PointOfInterestDto>>(pointsOfInterestForCity);
                //var pointOfInterestForCityResults= new List<PointOfInterestDto>();
                //foreach (var poi in pointsOfInterestForCity)
                //{
                //    pointOfInterestForCityResults.Add(new PointOfInterestDto()
                //    {
                //        Id = poi.Id,
                //        Name = poi.Name,
                //        Description = poi.Description
                //    });
                //}
                return Ok(pointOfInterestForCityResults);
                //  throw new Exception("Exception sample");
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} was not found");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occurred while getting points of interest for city with id {cityId}",ex);
                return StatusCode(500, "Error encountered");
            }
        }
        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    return NotFound();
                }

                var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
                if (pointOfInterest == null)
                {
                    return NotFound();
                }
                //var pointOfInterestResult = new PointOfInterestDto();
                //pointOfInterestResult.Id = pointOfInterest.Id;
                //pointOfInterestResult.Name = pointOfInterest.Name;
                //pointOfInterestResult.Description = pointOfInterest.Description;
                //var pointOfInterestResult = new PointOfInterestDto()
                //{
                //    Id = pointOfInterest.Id,
                //    Name = pointOfInterest.Name,
                //    Description = pointOfInterest.Description
                //};
                var pointOfInterestResult = Mapper.Map<Models.PointOfInterestDto>(pointOfInterest);
                return Ok(pointOfInterestResult);
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} was not found");
                //    return NotFound();
                //}
                //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(t => t.Id == id);
                //if (pointOfInterest == null)
                //{
                //    _logger.LogInformation($"Point of interest with id {id} was not found");
                //    return NotFound();
                //}
                //return Ok(pointOfInterest);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception occurred while getting point of interest for city with id {cityId} and point of interest {id}", ex);
                return StatusCode(500, "Error Encountered");
            }
        }
        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointofinterest)
        {
            if (pointofinterest == null)
            {
                return BadRequest();
            }
            if (pointofinterest.Name == pointofinterest.Description)
            {
                ModelState.AddModelError("Description", "The name & description must not be same");
            }
            if (!ModelState.IsValid)
            {// this will check for rules in creationdto model and return false if any 1 fails
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            //demo purpose, will improve in next module
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany
                (c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointofinterest.Name,
                Description = pointofinterest.Description

            };
            city.PointsOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                id = finalPointOfInterest.Id
            }, finalPointOfInterest);

        }
        [HttpPut("{cityid}/pointsofinterest/{id}")]
        //updates all columns in an entry. If input to update is not given for a column, 
        //it updates to default.pass empty value for description
        public IActionResult UpdatePointOfInterest(int cityid, int id,
           [FromBody] PointOfInterestForUpdateDto pointofinterest)
        {
            if (pointofinterest == null)
            {
                return BadRequest();
            }
            if (pointofinterest.Name == pointofinterest.Description)
            {
                ModelState.AddModelError("Description", "The name & description must not be same");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityid);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(t => t.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return BadRequest();
            }
            pointOfInterestFromStore.Name = pointofinterest.Name;
            pointOfInterestFromStore.Description = pointofinterest.Description;
            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]

        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(t => t.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description

            };
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The name & description must not be same");
            }
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            return NoContent();
        }


        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(t => t.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            _mailService.Send("Point of Interest Deleted",$"Point of interest of city {cityId} and id {id} was deleted");
            return NoContent();
        }
    }
}


        

   
