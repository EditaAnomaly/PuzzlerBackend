using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Models;
using RabbitMQ.Client;
using System.Text;

namespace ProductsService.Controllers;
[Route("[controller]")]
[ApiController]
public class ProductsController : Controller
{
    private readonly ProductsServiceContext _context;

    public ProductsController(ProductsServiceContext context)
    {
        _context = context;
    }

    // GET: getproducts
    [HttpGet("getproducts")]
    public async Task<ActionResult<List<Product>>> Get()
    {
        return Ok(await _context.Product.ToListAsync());
    }

    // GET: getproductbyid
    [HttpGet("getproductbyid")]
    public async Task<ActionResult<Product>> Details(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // POST: createproduct
    [HttpPost("createproduct")]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            var factory = new ConnectionFactory() { HostName = "rabbit" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    product.Id = Guid.NewGuid();
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    channel.QueueDeclare(queue: "product_details", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    string message = product.Id.ToString() + "|" + product.Usage.ToString();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "product_details", basicProperties: null, body: body);
                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
        }
        return product;
    }

    // POST: updateproduct
    [HttpPut("updateproduct")]
    public async Task<ActionResult<Product>> Edit(Guid id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return product;
    }

    // POST: removeproduct
    [HttpDelete("removeproduct")]
    public async Task<ActionResult<List<Product>>> Delete(Guid id)
    {
        var product = await _context.Product.FindAsync(id);
        _context.Product.Remove(product);
        await _context.SaveChangesAsync();
        return Ok(await _context.Product.ToListAsync());
    }

    private bool ProductExists(Guid id)
    {
        return _context.Product.Any(e => e.Id == id);
    }
}
