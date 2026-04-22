using GLMS_Core_Prototype.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Xunit;

namespace GLMS_Core_Prototype.Tests
{
    public class FileTests
    {
        private sealed class TestWebHostEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";
            public string ApplicationName { get; set; } = "Tests";
            public string WebRootPath { get; set; } = Path.Combine(Path.GetTempPath(), "glms-tests", "wwwroot");
            public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
            public string ContentRootPath { get; set; } = Path.GetTempPath();
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }

        [Fact]
        public void UploadNonPdf_ThrowsError()
        {
            var environment = new TestWebHostEnvironment();
            var fileService = new FileService(environment);
            var fakeFile = new FormFile(new MemoryStream(), 0, 0, "Data", "test.exe")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            Assert.Throws<InvalidOperationException>(() => fileService.SavePdfForContract(fakeFile, 1, null));
        }
    }
}
