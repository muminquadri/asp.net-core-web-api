using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }
        [HttpGet("{cityId}/pointsofinterest/{id}", Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(t => t.Id == id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
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
            { cityId=cityId, id=finalPointOfInterest.Id
            }, finalPointOfInterest);

        }
    }
}
