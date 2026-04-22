using Microsoft.AspNetCore.Hosting;

namespace GLMS_Core_Prototype.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string SavePdfForContract(IFormFile file, int contractId, string? existingPath)
        {
            if (file == null)
                throw new InvalidOperationException("Only PDF files allowed.");

            var extension = Path.GetExtension(file.FileName);
            if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only PDF files allowed.");
            }

            if (!string.IsNullOrWhiteSpace(existingPath))
            {
                var existingFullPath = Path.Combine(_environment.WebRootPath, existingPath.TrimStart('/', '\\'));
                if (File.Exists(existingFullPath))
                {
                    File.Delete(existingFullPath);
                }
            }

            var contractFolder = Path.Combine(_environment.WebRootPath, "uploads", "contracts", contractId.ToString());
            Directory.CreateDirectory(contractFolder);

            var fileName = "signed-agreement.pdf";
            var filePath = Path.Combine(contractFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return $"/uploads/contracts/{contractId}/{fileName}";
        }
    }
}
