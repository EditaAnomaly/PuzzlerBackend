using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ClientsService.Tests;

[TestClass]
public class Tests
{
    private ClientsController controller;

    [SetUp]
    public void Setup() {
        var mockedContext = new Mock<ClientsServiceContext>();
        var client = new Client("3fa85f64-5717-4562-b3fc-2c963f66afa6", "2022-06-06",
            "Maybelle", "Parker", "testadress", "062456423", 0, 0);
    }

    [TestMethod]
    public void TestCreateClient()
    {
        
        Assert.AreEqual("Linda", client.Name);
    }
}
