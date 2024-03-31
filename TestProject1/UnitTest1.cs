using ErrorWarningFilter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ErrorWarningFilter1.Tests
{
    [TestClass]
    public class PathProgramTests
    {
        [TestMethod]
        public void TestGetValidDirectoryPath_ValidInput()
        {
            // Arrange
            string tempPath = Path.Combine(Path.GetTempPath(), "TestDirectory");
            string message = "¬ведите путь каталога:";
            string userInput = tempPath + Environment.NewLine;

            // Create temporary directory
            Directory.CreateDirectory(tempPath);

            // Act
            using (StringReader sr = new StringReader(userInput))
            {
                Console.SetIn(sr);
                string result = PathProgram.GetValidDirectoryPath(message);
                // Assert
                Assert.AreEqual(tempPath, result);
            }

            // Clean up: delete temporary directory
            Directory.Delete(tempPath, true);
        }
    }
}