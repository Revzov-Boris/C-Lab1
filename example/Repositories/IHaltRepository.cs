using TravelApp.Entities;

namespace TravelApp.Repositories;

public interface IHaltRepository
{
    void Add(Halt halt);
    Halt? GetById(int id);
    IReadOnlyList<Halt> GetAll();
    IReadOnlyList<Halt> GetByRouteId(int routeId);
    IReadOnlyList<Halt> GetByCityId(int cityId);
    void Update(Halt halt);
    void Delete(int id);
}