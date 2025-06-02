using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.Services;

public interface ITripsService
{
    Task<object> GetTrips(int page = 1, int pageSize = 10);
    Task<bool> AssignClientToTrip(int idTrip, AssignClientDto dto);
}