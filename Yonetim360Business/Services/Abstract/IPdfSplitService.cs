using Yonetim360Business.DTO.DocumentProcessing;

namespace Yonetim360Business.Services.Abstract
{
    public interface IPdfSplitService
    {
        Task<List<ExtractedPdfInfoDto>> SplitPdfByDocumentTypeAsync(string originalPdfPath, List<PageGroupDto> pageGroups, string outputDirectory);
    }
}
