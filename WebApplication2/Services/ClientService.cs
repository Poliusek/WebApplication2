using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Services;

public class ClientService : IClientService
{
    private readonly MasterContext _context;

    public ClientService(MasterContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteClient(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
            return false;

        if (client.ClientTrips.Any())
            return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}