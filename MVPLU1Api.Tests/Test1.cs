namespace MVPLU1Api.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            int a = 1;
            int b = 1;

            // Act
            int result = a + b;

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
