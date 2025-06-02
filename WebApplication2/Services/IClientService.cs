namespace WebApplication2.Services;

public interface IClientService
{
    Task<bool> DeleteClient(int idClient);
}