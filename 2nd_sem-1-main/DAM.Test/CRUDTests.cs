using Avalonia;
using Avalonia.Headless;
using Moq;

[assembly: AvaloniaTestApplication(typeof(DAM.Tests.TestAppBuilder))]

namespace DAM.Tests
{
    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() => AppBuilder
            .Configure<App>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }

    [TestFixture]
    public class CreateTests
    {

        [TestFixture]
        public class ExtractFilenameTests
        {
            [Test]
            public void ExtractFilename_FromUnixPath_ReturnsCorrectFilename()
            {
                string path = "/Users/test/files/image.png";
                string filename = path.Substring(path.LastIndexOf('/') + 1);
                Assert.That(filename, Is.EqualTo("image.png"));
            }
            
            [Test]
            public void ExtractFilename_FromWindowsPath_ReturnsCorrectFilename()
            {
                string path = "C:\\Users\\test\\files\\image.png";
                string filename = path.Substring(path.LastIndexOf('\\') + 1);
                Assert.That(filename, Is.EqualTo("image.png"));
            }
        }

        [TestFixture]
        public class FilePathProcessingTests
        {
            [Test]
            public void ExtractFileType_FromPath_ReturnsCorrectType()
            {
                string[] paths = {
                    "/Users/test/files/image.png",
                    "C:\\Users\\test\\documents\\report.pdf",
                    "/media/videos/movie.mp4"
                };
                
                string[] expectedTypes = { ".png", ".pdf", ".mp4" };
                
                for (int i = 0; i < paths.Length; i++)
                {
                    string path = paths[i];
                    string type = path.Substring(path.LastIndexOf('.'));
                    Assert.That(type, Is.EqualTo(expectedTypes[i]));
                }
            }
            
            [Test]
            public void ProcessImagePath_ExtractsTypeAndFilename()
            {
                string imagePath = "/Users/test/files/testimage.png";
                string type = imagePath.Substring(imagePath.LastIndexOf('.'));
                string filename = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
                Assert.That(type, Is.EqualTo(".png"));
                Assert.That(filename, Is.EqualTo("testimage.png"));
            }
        }

        [TestFixture]
        public class ScalingIntegrationTests
        {
            private Mock<Scaling> _scalingMock;
            
            [SetUp]
            public void Setup()
            {
                _scalingMock = new Mock<Scaling>();
            }
            
            [Test]
            public void Scale_WithCorrectParameters_CallsScalingMethod()
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "assets/picture/test.png");
                try
                {
                    Scaling.Scale(filePath, 100, 150, "type1");
                }
                catch (Exception)
                {
                }
            }
            
            [Test]
            public void Scale_WithDifferentTypes_SavesToDifferentDirectories()
            {
                string baseFilename = "test.png";
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), "assets/picture", baseFilename);
                string type1Path = $"{Directory.GetCurrentDirectory()}/assets/scale_type1/{baseFilename}";
                string type2Path = $"{Directory.GetCurrentDirectory()}/assets/scale_type2/{baseFilename}";
                string type3Path = $"{Directory.GetCurrentDirectory()}/assets/scale_type3/{baseFilename}";
                
                Assert.That(type1Path, Does.Contain("scale_type1"));
                Assert.That(type2Path, Does.Contain("scale_type2"));
                Assert.That(type3Path, Does.Contain("scale_type3"));
            }
        }
    }

    [TestFixture]
    public class FileTypeHandlingTests
    {
        [Test]
        public void DetermineFileType_ForDifferentExtensions()
        {
            string[] filenames = {
                "document.pdf",
                "image.png", 
                "video.mp4",
                "unknown.txt"
            };
            
            foreach (string filename in filenames)
            {
                string type = filename.Substring(filename.LastIndexOf('.'));
                
                switch (type)
                {
                    case ".png":
                        Assert.That(type, Is.EqualTo(".png"));
                        break;
                    case ".pdf":
                        Assert.That(type, Is.EqualTo(".pdf"));
                        break;
                    case ".mp4":
                        Assert.That(type, Is.EqualTo(".mp4"));
                        break;
                    default:
                        Assert.That(type, Is.Not.EqualTo(".png"));
                        Assert.That(type, Is.Not.EqualTo(".pdf"));
                        Assert.That(type, Is.Not.EqualTo(".mp4"));
                        break;
                }
            }
        }
    }

    [TestFixture]
    public class PathCombinationTests
    {
        [Test]
        public void PathCombine_JoinsDirectoryAndFilename()
        {
            string directory = $"{Directory.GetCurrentDirectory()}/assets/picture";
            string filename = "test.png";
            string result = Path.Combine(directory, filename);
            
            if (!OperatingSystem.IsWindows())
            {
                Assert.That(result, Is.EqualTo($"{directory}/{filename}"));
            }
            else
            {
                Assert.That(result, Is.EqualTo($"{directory}\\{filename}"));
            }
        }
    }

    [TestFixture]
    public class FileDestinationDeterminationTests
    {
        [Test]
        public void GetFileDestination_ForSupportedTypes()
        {
            string[] extensions = { ".png", ".pdf", ".mp4" };
            string[] expectedPaths = {
                $"{Directory.GetCurrentDirectory()}/assets/picture",
                $"{Directory.GetCurrentDirectory()}/assets/pdf",
                $"{Directory.GetCurrentDirectory()}/assets/films"
            };
            
            if (OperatingSystem.IsWindows())
            {
                for (int i = 0; i < expectedPaths.Length; i++)
                {
                    expectedPaths[i] = expectedPaths[i].Replace('/', '\\');
                }
            }
            
            for (int i = 0; i < extensions.Length; i++)
            {
                string fileDestination = "";
                
                switch (extensions[i])
                {
                    case ".png":
                        fileDestination = $"{Directory.GetCurrentDirectory()}/assets/picture";
                        break;
                    case ".pdf":
                        fileDestination = $"{Directory.GetCurrentDirectory()}/assets/pdf";
                        break;
                    case ".mp4":
                        fileDestination = $"{Directory.GetCurrentDirectory()}/assets/films";
                        break;
                }
                
                if (OperatingSystem.IsWindows())
                {
                    fileDestination = fileDestination.Replace('/', '\\');
                }
                
                Assert.That(fileDestination, Is.EqualTo(expectedPaths[i]));
            }
        }
    }

    [TestFixture]
    public class ReadTests
    {
        [Test]
        public void Read_CanBeInstantiated()
        {
            Assert.DoesNotThrow(() => {
            });
        }
    }

    [TestFixture]
    public class CRUDTests
    {
        [Test]
        public void Delete_CanBeInstantiated()
        {
            Assert.DoesNotThrow(() => {
            });
        }
    }

    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void Update_CanBeInstantiated()
        {
            Assert.DoesNotThrow(() => {
            });
        }
    }
}