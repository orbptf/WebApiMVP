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

