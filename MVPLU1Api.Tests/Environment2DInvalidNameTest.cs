using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectMap.WebApi.Controllers;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

namespace MVPLU1Api.Tests;

[TestClass]
public class Environment2DInvalidNameTest
{
    [TestMethod]
    public async Task Environment2DInvalidName()
    {
        //Arrange
        string toShortWorldNameText = "";
        string toLongWorldNameText = "MMMMMMMMMMMMMMMMMMMMMMMMMm"; //25x M, 1x m
        string validWorldNameText = "test";

        var environmentRepository = new Mock<IEnvironment2DRepository>();
        var logger = new Mock<ILogger<Environment2DController>>();
        var controller = new Environment2DController(environmentRepository.Object, logger.Object);

        var toShortWorldName = new Environment2D { Name = toShortWorldNameText };
        var toLongWorldName = new Environment2D { Name = toLongWorldNameText };
        var validWorldName = new Environment2D { Name = validWorldNameText };

        //Act

        var resultToShort = await controller.Add(toShortWorldName);
        var resultToLong = await controller.Add(toLongWorldName);
        var resultValid = await controller.Add(validWorldName);

        //Assert
        Assert.IsInstanceOfType(resultToShort, typeof(BadRequestObjectResult));
        Assert.IsInstanceOfType(resultToLong, typeof(BadRequestObjectResult));
        Assert.IsInstanceOfType(resultValid, typeof(CreatedResult));
    }
}


    //[TestMethod]
    //public async Task Get_EnvironmentFromOtherUser_Returns404NotFound()
    //{
    //    // Arrange
    //    // Zorgen dat mijn repo geen resultaat geeft
    //    var userId1 = Guid.NewGuid().ToString();
    //    var userId2 = Guid.NewGuid().ToString();
    //    var environmentId = Guid.NewGuid();
    //    var environmentRepository = new Mock<IEnvironmentRepository>();
    //    var objectRepository = new Mock<IObjectRepository>();
    //    var userRepository = new Mock<IAuthenticationService>();

    //    userRepository.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns(userId1);
    //    environmentRepository.Setup(x => x.ReadAsync(environmentId)).ReturnsAsync(new Environment2D { OwnerUserId = userId2 });


    //    var controller = new EnvironmentsController(environmentRepository.Object, objectRepository.Object, userRepository.Object);

    //    // Act
    //    var response = await controller.GetByIdAsync(environmentId);

    //    // Assert
    //    Assert.IsInstanceOfType(response.Result, out NotFoundObjectResult notFoundObjectResult);

    //}

