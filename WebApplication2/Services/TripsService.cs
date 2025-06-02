using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.Services;

public class TripsService : ITripsService
{
    private readonly MasterContext _context;

    public TripsService(MasterContext context)
    {
        _context = context;
    }

    public async Task<object> GetTrips(int page = 1, int pageSize = 10)
    {
        var query = _context.Trips
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom);

        var totalTrips = await query.CountAsync();
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new ReturnTripDTO{
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => c.Name).ToList(),
                Clients = t.ClientTrips.Select(ct => new ReturnClient() {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync<ReturnTripDTO>();

        return new {
            pageNum = page,
            pageSize = pageSize,
            allPages = allPages,
            trips = trips
        };
    }

    public async Task<bool> AssignClientToTrip(int idTrip, AssignClientDto dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
            return false;

        if (trip.DateFrom <= DateTime.Now)
            return false;

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (client != null)
        {
            var alreadyAssigned = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
            if (alreadyAssigned)
                return false;
            return false;
        }

        client = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return true;
    }
}