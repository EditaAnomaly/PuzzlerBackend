namespace ClientsService.Models
{
    public interface IClientRepo : IDisposable
    {
        List<Client> GetClients();
        Client GetClientById(Guid id);
        void InsertClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Guid id);
        void Save();
    }
}
