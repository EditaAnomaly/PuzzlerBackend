using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientsService.Models;
using ClientsService.Data;
using RabbitMQ.Client;
using System.Text;

namespace ClientsService.Controllers;   
[Route("[controller]")]
[ApiController]
public class ClientsController : Controller
{
    private readonly ClientsServiceContext _context;

    public ClientsController(ClientsServiceContext context)
    {
        _context = context;
    }

    // GET: getclients
    [HttpGet("getclients")]
    public async Task<ActionResult<List<Client>>> Get()
    {
        return Ok(await _context.Client.ToListAsync());
    }

    // GET: getclientbyid
    [HttpGet("getclientbyid")]
    public async Task<ActionResult<Client>> Details(Guid id)
    {
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
                    _context.Add(client);
                    await _context.SaveChangesAsync();
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
                _context.Update(client);
                await _context.SaveChangesAsync();
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
        var client = await _context.Client.FindAsync(id);
        _context.Client.Remove(client);
        await _context.SaveChangesAsync();
        return Ok(await _context.Client.ToListAsync());
    }

    private bool ClientExists(Guid id)
    {
        return _context.Client.Any(e => e.Id == id);
    }
}
