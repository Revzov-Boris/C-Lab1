using TravelApp.Entities;

namespace TravelApp.Services;

public interface ICityService
{
    void CreateCity(string name, int timeOffset, string country, string region);
    City? GetCity(int id);
    IReadOnlyList<City> GetAllCities();
    IReadOnlyList<City> SearchCitiesByName(string searchTerm);
    IReadOnlyList<City> GetCitiesByCountry(string country);
    IReadOnlyList<City> GetCitiesSortedByTimeOffset();
    void UpdateCity(int id, string name, int timeOffset, string country, string region);
    void DeleteCity(int id);
}