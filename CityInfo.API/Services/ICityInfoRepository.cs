using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool IncludePointsOfInterest);

        IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId);

        PointOfInterest GetPointOfInterestsForCity(int cityId, int pointOfInterestId);

        bool CityExists(int cityId);
    }
}
