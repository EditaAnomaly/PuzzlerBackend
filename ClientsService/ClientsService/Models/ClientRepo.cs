using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ClientsService.Data;

namespace ClientsService.Models
{
    public class ClientRepo : IClientRepo, IDisposable
    {
        private readonly ClientsServiceContext context;

        public ClientRepo(ClientsServiceContext context)
        {
            this.context = context;
        }

        public List<Client> GetClients()
        {
            return context.Client.ToList();
        }

        public Client GetClientById(Guid id)
        {
            return context.Client.Find(id);
        }

        public void InsertClient(Client client)
        {
            context.Client.Add(client);
        }

        public void DeleteClient(Guid id)
        {
            Client clienttoremove = context.Client.Find(id);
            if (clienttoremove!=null)
            context.Client.Remove(clienttoremove);
        }

        public void UpdateClient(Client client)
        {
            context.Entry(client).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}