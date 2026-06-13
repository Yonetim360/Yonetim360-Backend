using Microsoft.AspNetCore.Http;

namespace Yonetim360Business.Services.Abstract
{
    public interface IDocumentStorageService
    {
        Task<(string StoredFileName, string StoredPath, string RelativeDirectory)> SaveUploadedDocumentAsync(
            Guid tenantId,
            IFormFile file,
            CancellationToken cancellationToken);

        string EnsureExtractedDirectory(Guid tenantId, Guid uploadedDocumentId);
    }
}
