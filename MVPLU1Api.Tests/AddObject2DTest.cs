using Moq;
using ProjectMap.WebApi.Interfaces;
using ProjectMap.WebApi.Models;

namespace MVPLU1Api.Tests;

[TestClass]
public class AddObject2DTest
{
    [TestMethod]
    public void AddObject2D()
    {
        // Arrange
        var object2DRepository = new Mock<IObject2DRepository>();
        var newObject = new Object2D { Id = Guid.NewGuid(), Name = "TestObject", EnvironmentId = Guid.NewGuid() };

        // Act
        object2DRepository.Object.InsertAsync(newObject);

        // Assert
        object2DRepository.Verify(repo => repo.InsertAsync(It.IsAny<Object2D>()), Times.Once);
    }
}
