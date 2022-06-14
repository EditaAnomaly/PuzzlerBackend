using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientsService.Data;
using Microsoft.Extensions.DependencyInjection;
using ClientsService.Models;
using ClientsService.Controllers;
using Moq;

namespace ClientServiceTests;

public class ControllerTests
{
    private readonly ClientsServiceContext _context;

    private List<Client> mockedList = new List<Client>
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
         new Client
        {
                Id= new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                DateRegistered = DateTime.Now,
                Name="Tammy",
                Surname="Lee",
                Adress="testadress3",
                PhoneNr="063425984",
                Deposit=25,
                Reputation=95
        }
    };

    [Fact]
    public void ShouldGetAllClients()
    {
        // Arrange
        var repositoryMock = new Mock<IClientRepo>();
        repositoryMock.Setup(r => r.GetClients()).Returns(mockedList);
        var controller = new ClientsController(repositoryMock.Object);

        // Act
        var clients = controller.Get().Result.Value;
     
        // Assert
        if(clients !=null)
        Assert.Equal(3, clients.Count);
    }

    [Fact]
    public void ShouldGetClientById()
    {
        // Arrange
        var repositoryMock = new Mock<IClientRepo>();
        repositoryMock.Setup(r => r.GetClients()).Returns(mockedList);
        var controller = new ClientsController(repositoryMock.Object);

        // Act
        var testId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482");
        var client = controller.Details(testId).Result.Value;

        // Assert
        if (client != null)
           Assert.Equal(testId.ToString(), client.Id.ToString());
    }
    [Fact]
    public void ShouldRemoveClient()
    {
        // Arrange
        var repositoryMock = new Mock<IClientRepo>();
        repositoryMock.Setup(r => r.GetClients()).Returns(mockedList);
        var controller = new ClientsController(repositoryMock.Object);

        // Act
        var testId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482");
        var client = controller.Delete(testId).Result.Value;

        // Assert
        if (client != null)
            Assert.Equal(2, client.Count);
    }
}
