﻿using CityInfo.API.Context;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context=context ?? throw new ArgumentNullException(nameof(context));
        }
        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c=>c.Name).ToList();
        }

        public City GetCity(int cityId,bool IncludePointsOfInterest)
        {
            if(IncludePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestsForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterests.Where(p=> p.CityId==cityId && p.Id==pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId)
        {
            return _context.PointOfInterests.Where(p=>p.CityId==cityId).ToList();  
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c=>c.Id==cityId);
        }
    }
}
