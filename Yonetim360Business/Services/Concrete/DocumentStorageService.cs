using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class DocumentStorageService : IDocumentStorageService
    {
        private readonly DocumentStorageOptions _options;

        public DocumentStorageService(IOptions<DocumentStorageOptions> options)
        {
            _options = options.Value;
        }

        public async Task<(string StoredFileName, string StoredPath, string RelativeDirectory)> SaveUploadedDocumentAsync(
            Guid tenantId,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.RootPath))
            {
                throw new InvalidOperationException("Document storage root path is not configured.");
            }

            var extension = Path.GetExtension(file.FileName);
            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var now = DateTime.UtcNow;
            var relativeDirectory = Path.Combine(tenantId.ToString(), "uploaded", now.Year.ToString("0000"), now.Month.ToString("00"));
            var absoluteDirectory = Path.Combine(_options.RootPath, relativeDirectory);
            Directory.CreateDirectory(absoluteDirectory);

            var storedPath = Path.Combine(absoluteDirectory, storedFileName);
            await using var stream = new FileStream(storedPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream, cancellationToken);

            return (storedFileName, storedPath, relativeDirectory);
        }

        public string EnsureExtractedDirectory(Guid tenantId, Guid uploadedDocumentId)
        {
            if (string.IsNullOrWhiteSpace(_options.RootPath))
            {
                throw new InvalidOperationException("Document storage root path is not configured.");
            }

            var directory = Path.Combine(_options.RootPath, tenantId.ToString(), "processed", uploadedDocumentId.ToString(), "extracted");
            Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
