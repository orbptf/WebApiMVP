using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

[TestClass]
public class Environment2DMaximumNumberOfWorlds
{
    [TestMethod]
    public async Task Environment2DMaximumWorlds()
    {
        // Arrange
        string userMail = "test@test.com";

        Mock<IEnvironment2DRepository> environmentRepository = new Mock<IEnvironment2DRepository>();
        Mock<ILogger<Environment2DController>> logger = new Mock<ILogger<Environment2DController>>();
        Environment2DController controller = new Environment2DController(environmentRepository.Object, logger.Object);

        List<Environment2D> userWorlds = new List<Environment2D>();

        // Voeg tot 6 werelden toe en controleer steeds of het mag
        for (int i = 1; i <= 6; i++)
        {
            Environment2D newWorld = new Environment2D { Name = $"World {i}", usermail = userMail };

            // Zorg ervoor dat de mock nooit null retourneert
            environmentRepository.Setup(r => r.ReadByUserMailAsync(userMail))
                .ReturnsAsync(userWorlds ?? new List<Environment2D>());

            ActionResult result = await controller.Add(newWorld);

            if (i <= 5)
            {
                Assert.IsInstanceOfType(result, typeof(CreatedResult));
                userWorlds.Add(newWorld);
            }
            else
            {
                Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            }
        }

        environmentRepository.Verify(r => r.ReadByUserMailAsync(userMail), Times.Exactly(6));
    }
}
