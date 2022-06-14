using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientsService.Models;
using RabbitMQ.Client;
using System.Text;

namespace ClientsService.Controllers;   
[Route("[controller]")]
[ApiController]
public class ClientsController : Controller
{
    private readonly IClientRepo _repo; 


    public ClientsController(IClientRepo repo)
    {
        _repo = repo;
    }

    // GET: getclients
    [HttpGet("getclients")]
    public async Task<ActionResult<List<Client>>> Get()
    {
        return Ok( _repo.GetClients());
    }

    // GET: getclientbyid
    [HttpGet("getclientbyid")]
    public async Task<ActionResult<Client>> Details(Guid id)
    {
        var client = _repo.GetClientById(id);
        if (client == null)
        {
            return NotFound();
        }
        return client;
    }

    // POST: createclient
    [HttpPost("createclient")]
    public async Task<ActionResult<Client>> Create(Client client)
    {
         if (ModelState.IsValid)
         {
            var factory = new ConnectionFactory() { HostName = "rabbit" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    client.Id = Guid.NewGuid();
                    _repo.InsertClient(client);
                    _repo.Save();
                    channel.QueueDeclare(queue: "client_details", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string message = client.Id.ToString() + "|" + client.Name.ToString() + "|" + client.Surname.ToString() + "|" + client.Deposit.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "client_details", basicProperties: null, body: body);
                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
         }
        return client;
    }

    // PUT: updateclient
    [HttpPut("updateclient")]
    public async Task<ActionResult<Client>> Edit(Guid id, Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _repo.UpdateClient(client);
                _repo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return client;
    }


    // POST: removeclientbyid
    [HttpDelete("removeclientbyid")]
    public async Task<ActionResult<List<Client>>> Delete(Guid id)
    {
        _repo.DeleteClient(id);
        _repo.Save();
        return Ok( _repo.GetClients());
    }

    private bool ClientExists(Guid id)
    {
        if (_repo.GetClientById(id) !=null)
            return true;
        else
            return false;
    }
}
