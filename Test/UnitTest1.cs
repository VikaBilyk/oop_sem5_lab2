using Moq;
using WebSite;
using ILogger = Serilog.ILogger;

namespace Test
{
    [TestFixture]
    public class WebSiteManagerTests
    {
        private WebSiteManager _webSiteManager;
        private Mock<ILogger> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            // Initialize mock logger
            _mockLogger = new Mock<ILogger>();
            _webSiteManager = new WebSiteManager(_mockLogger.Object);
        }

        [Test]
        public void SelectInfoFromUserInput_ValidInput_ReturnsCorrectEnum()
        {
            // Arrange
            int input = 1; // Expecting Advertising
            WebSiteType expectedType = WebSiteType.Advertising;

            // Act
            WebSiteType result = _webSiteManager.SelectInfoFromUserInput(input);

            // Assert
            Assert.That(result, Is.EqualTo(expectedType));
        }

        [Test]
        public void SelectInfoFromUserInput_InvalidInput_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            int invalidInput = 99;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _webSiteManager.SelectInfoFromUserInput(invalidInput));
        }

        [Test]
        public void GetInfoFromUserInput_ValidInput_ReturnsCorrectValue()
        {
            // Arrange
            string userInput = "2\n";  // Simulate user input of 2
            var inputReader = new StringReader(userInput);
            Console.SetIn(inputReader);

            // Act
            int result = _webSiteManager.GetInfoFromUserInput("Select option", 4);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GetAndSaveToXml_FileExists_LoadsAndLogs()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, "<Site></Site>");

            // Act
            _webSiteManager.GetAndSaveToXml(tempFilePath);

            // Assert
            _mockLogger.Verify(logger => logger.Information("Loaded existing XML file: {FilePath}", tempFilePath), Times.Once);

            // Clean up temp file
            File.Delete(tempFilePath);
        }

        [Test]
        public void GetAndSaveToXml_FileDoesNotExist_CreatesNewFileAndLogs()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "nonexistent.xml");

            // Act
            _webSiteManager.GetAndSaveToXml(tempFilePath);

            // Assert
            _mockLogger.Verify(logger => logger.Information("Created new XML document."), Times.Once);

            // Clean up temp file if created
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
}
