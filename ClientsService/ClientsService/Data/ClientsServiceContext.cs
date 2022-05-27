using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClientsService.Models;

namespace ClientsService.Data
{
    public class ClientsServiceContext : DbContext
    {
        public ClientsServiceContext (DbContextOptions<ClientsServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("ClientsServiceContext");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
