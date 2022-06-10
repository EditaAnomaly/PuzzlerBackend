using ClientsService.Models;
namespace ClientsService.Services;

public interface IService
{
    Task<List<Client>> GetClientsAsync();
    Task<Client?> GetClientAsync(Guid id);
    Task CreateAsync(Client client);  
    Task UpdateAsync(Guid id, Client client);
    Task DeleteAsync(Guid id);
}
