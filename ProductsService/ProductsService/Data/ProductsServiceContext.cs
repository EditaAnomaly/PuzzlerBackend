using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsService.Models;

namespace ProductsService.Data;

public class ProductsServiceContext : DbContext
{
    public ProductsServiceContext (DbContextOptions<ProductsServiceContext> options)
        : base(options)
    {
    }

    public DbSet<ProductsService.Models.Product>? Product { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        var connectionString = configuration.GetConnectionString("ProductsServiceContext");
        optionsBuilder.UseSqlServer(connectionString);
    }
}
