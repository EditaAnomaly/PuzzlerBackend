using ClientsService.Models;

namespace ClientsService.Data;

public static class ClientDbInitializer
{
    public static void Initialize(ClientsServiceContext context)
    { 
        context.Database.EnsureCreated();

        if (context.Client.Any())
        {
            return;
        }

        var clients = new[]
        {
            new Client 
            {
                Id=Guid.NewGuid(), 
                DateRegistered=DateTime.Now, 
                Name="Maybelle",
                Surname="Parker", 
                Adress="testadress", 
                PhoneNr="062456423", 
                Deposit=10, 
                Reputation=100 
            },
            new Client 
            { 
                Id=Guid.NewGuid(), 
                DateRegistered = DateTime.Now, 
                Name="Merlin", 
                Surname="Jones", 
                Adress="testadress2", 
                PhoneNr="063425274", 
                Deposit=15, 
                Reputation=98 
            },
        };

        foreach (var client in clients)
        {
            context.Client.Add(client);
        }
        context.SaveChanges();
    }

}
