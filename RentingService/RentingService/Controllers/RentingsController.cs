using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentingService;
using RentingService.Data;

namespace RentingService.Controllers;

public class RentingsController : Controller
{
    private readonly RentingServiceContext _context;
 
    public RentingsController(RentingServiceContext context)
    {
        _context = context;
    }

    // GET: getrentings
    [HttpGet("getrenting")]
    public async Task<ActionResult<List<Renting>>> Get()
    {
        return Ok(await _context.Renting.ToListAsync());
    }

    // GET: getrentingbyid
    [HttpGet("getrentingbyid")]
    public async Task<ActionResult<Renting>> Details(Guid id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var renting = await _context.Renting
            .FirstOrDefaultAsync(m => m.Id == id);
        if (renting == null)
        {
            return NotFound();
        }
        return renting;
    }

    // POST: createrenting
    [HttpPost("createrenting")]
    public async Task<ActionResult<Renting>> Create(Renting renting)
    {
        if (ModelState.IsValid)
        {
            renting.Id = Guid.NewGuid();
            _context.Add(renting);
            await _context.SaveChangesAsync();
        }
        return renting;
    }

    // PUT: updaterenting
    [HttpPut("updaterenting")]
    public async Task<ActionResult<Renting>> Edit(Guid id, Renting renting)
    {
        if (id != renting.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(renting);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentingExists(renting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return renting;
    }

    // Delete: removerenting
    [HttpDelete("removerenting")]
    public async Task<ActionResult<List<Renting>>> DeleteConfirmed(Guid id)
    {
        var renting = await _context.Renting.FindAsync(id);
        _context.Renting.Remove(renting);
        await _context.SaveChangesAsync();
        return Ok(await _context.Renting.ToListAsync());
    }

    private bool RentingExists(Guid id)
    {
        return (_context.Renting?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
