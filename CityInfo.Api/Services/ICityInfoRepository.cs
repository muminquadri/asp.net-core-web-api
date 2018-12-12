﻿using CityInfo.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
     public interface ICityInfoRepository
     {
         //IQueryable<City> GetCities();
         bool CityExists(int cityId);
         IEnumerable<City> GetCities();
         City GetCity(int cityId, bool includePointsOfInterest);
         IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
         PointOfInterest GetPointOfInterestForCity(int cityId, int PointOfInterestId);
         void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
         void DeletePointOfInterest(PointOfInterest pointOfInterest);
         bool Save();

     }
}
