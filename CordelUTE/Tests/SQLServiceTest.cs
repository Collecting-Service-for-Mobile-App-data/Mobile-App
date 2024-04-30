using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace MauiApp1.Tests
{
    [TestFixture]
    public class SQLServiceTests
    {
        private SQLService _sqlService;

        [SetUp]
        public void Setup()
        {
            _sqlService = new SQLService();
        }

        [Test]
        public async Task ConfigureDatabase_Should_CopyDatabase()
        {
            // Arrange

            // Act
            await _sqlService.ConfigureDatabase();

            // Assert
            string copyDatabasePath = _sqlService.GetPathToCopyDatabase();
            Assert.IsTrue(File.Exists(copyDatabasePath));
        }

        [Test]
        public async Task ConfigureDatabase_Should_CreateMyTable()
        {
            // Arrange

            // Act
            await _sqlService.ConfigureDatabase();

            // Assert
            string mainDatabasePath = _sqlService.GetPathToDatabase();
            Assert.IsTrue(File.Exists(mainDatabasePath));

            // TODO: Add additional assertions to verify that the 'MyTable' is created
        }

        [Test]
        public async Task ConfigureDatabase_Should_CloseConnection()
        {
            // Arrange

            // Act
            await _sqlService.ConfigureDatabase();

            // Assert
            // TODO: Add assertion to verify that the database connection is closed
        }

        [Test]
        public async Task CopyDatabase_Should_CopyFile()
        {
            // Arrange

            // Act
            await _sqlService.CopyDatabase();

            // Assert
            string sourceFile = _sqlService.GetPathToDatabase() + "\\database.db";
            string destinationFile = _sqlService.GetPathToCopyDatabase() + "\\database.db";
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.AreEqual(new FileInfo(sourceFile).Length, new FileInfo(destinationFile).Length);
        }

        // TODO: Add more tests for other methods in SQLService class
    }
}