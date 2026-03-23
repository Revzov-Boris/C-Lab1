using TravelApp.Entities;

namespace TravelApp.Repositories;

public interface ICityRepository
{
    void Add(City city);
    City? GetById(int id);
    IReadOnlyList<City> GetAll();
    void Update(City city);
    void Delete(int id);
    bool Exists(int id);
}