using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentingService;
using RentingService.Model;

namespace RentingService.Data;

public class RentingServiceContext : DbContext
{
    public RentingServiceContext (DbContextOptions<RentingServiceContext> options)
        : base(options)
    {
    }

    public DbSet<Renting>? Renting { get; set; }
    public DbSet<ClientDetails>? ClientDetails { get; set; }
    public DbSet<ProductDetails>? ProductDetails { get; set; }
}
