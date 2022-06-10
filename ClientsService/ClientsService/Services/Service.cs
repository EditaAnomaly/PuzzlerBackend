using ClientsService.Data;
using ClientsService.Models;
using RabbitMQ.Client;
using System.Text;

namespace ClientsService.Services;

public class Service : IService
{
    private readonly ClientsServiceContext _context;
    public Service() { }
    public Service(ClientsServiceContext context)
    {
        _context = context;
    }
    public async Task<Client?> CreateAsync(Client client)
    {
        var factory = new ConnectionFactory() { HostName = "rabbit" };
        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                client.Id = Guid.NewGuid();
                _context.Add(client);
                await _context.SaveChangesAsync();
                channel.QueueDeclare(queue: "client_details", durable: false, exclusive: false, autoDelete: false, arguments: null);
                string message = client.Id.ToString() + "|" + client.Name.ToString() + "|" + client.Surname.ToString() + "|" + client.Deposit.ToString();
                var body = Encoding.UTF8.GetBytes(message);
              
                channel.BasicPublish(exchange: "", routingKey: "client_details", basicProperties: null, body: body);
                Console.WriteLine("[x] Sent {0}", message);
            }
        }
        return client;
    }

    public async Task<Client> UpdateAsync(Guid id, Client client)
    {
        if (id != client.Id)
        {
            return null;
        }
        else
        {
          _context.Update(client);
          await _context.SaveChangesAsync();
        }
        return client;
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _context.Client.FindAsync(id);
        _context.Client.Remove(client);
        await _context.SaveChangesAsync();
        return Ok(await _context.Client.ToListAsync());
    }

    public async Task<Client?> GetClientAsync(string clientId)
    {
        //var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
        var client = await _context.Client.FindAsync(clientId);
        return client;
    }

    public async Task<List<Client>> GetClientsAsync()
    {
        throw new NotImplementedException();
    }
}
