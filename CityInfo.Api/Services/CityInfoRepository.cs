using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace CityInfo.Api.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }
        public IEnumerable<City> GetCities()
        {
           return _context.Cities.OrderBy(c => c.Name).ToList();
           // return _context.Cities.Include(c => c.PointsOfInterest).OrderBy(c => c.Name).ToList();
            // return _context.Cities.AsEnumerable().ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int PointOfInterestId)
        {
            return _context.PointsOfInterest.FirstOrDefault(c => c.CityId == cityId && c.Id == PointOfInterestId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId).ToList();
        }
    }
}
