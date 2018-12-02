using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Services;
using CityInfo.Api.Models;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class CitiesController:Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet()]
        public IActionResult GetCities()
        {
          // return Ok(CitiesDataStore.Current.Cities);
          //  return _cityInfoRepository.GetCities(); IF public IEnumerable<City> is return type of method
            var cityEntities = _cityInfoRepository.GetCities();
            //   return Ok(cityEntities); include pointsofinterest
            var results = new List<CityWithoutPointOfInterestDto>();
            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointOfInterestDto()
                {
                    Id = cityEntity.Id,
                    Name = cityEntity.Name,
                    Description = cityEntity.Description

                });

            }
            return Ok(results);


        }
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {         
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(t => t.Id == id);
            if (cityToReturn == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cityToReturn);
            }
        }
    }
}
