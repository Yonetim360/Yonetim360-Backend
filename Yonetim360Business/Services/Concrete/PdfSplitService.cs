using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class PdfSplitService : IPdfSplitService
    {
        public Task<List<ExtractedPdfInfoDto>> SplitPdfByDocumentTypeAsync(string originalPdfPath, List<PageGroupDto> pageGroups, string outputDirectory)
        {
            var results = new List<ExtractedPdfInfoDto>();

            using var originalDocument = PdfReader.Open(originalPdfPath, PdfDocumentOpenMode.Import);
            foreach (var group in pageGroups)
            {
                using var newDocument = new PdfDocument();

                for (var pageNumber = group.StartPage; pageNumber <= group.EndPage; pageNumber++)
                {
                    if (pageNumber <= 0 || pageNumber > originalDocument.PageCount)
                    {
                        continue;
                    }

                    newDocument.AddPage(originalDocument.Pages[pageNumber - 1]);
                }

                if (newDocument.PageCount == 0)
                {
                    results.Add(new ExtractedPdfInfoDto
                    {
                        DocumentType = group.DocumentType,
                        StartPage = group.StartPage,
                        EndPage = group.EndPage,
                        IsSuccessful = false,
                        ErrorMessage = "No pages found for this document group.",
                        AverageConfidence = group.AverageConfidence,
                        CombinedKeywords = group.Keywords
                    });
                    continue;
                }

                var fileName = GenerateFileName(group.DocumentType);
                var outputPath = Path.Combine(outputDirectory, fileName);
                var extractedPageCount = newDocument.PageCount;
                newDocument.Save(outputPath);

                results.Add(new ExtractedPdfInfoDto
                {
                    FileName = fileName,
                    FilePath = outputPath,
                    DocumentType = group.DocumentType,
                    StartPage = group.StartPage,
                    EndPage = group.EndPage,
                    PageCount = extractedPageCount,
                    AverageConfidence = group.AverageConfidence,
                    CombinedKeywords = group.Keywords,
                    IsSuccessful = true
                });
            }

            return Task.FromResult(results);
        }

        private static string GenerateFileName(DocumentFileType documentType)
        {
            return $"{documentType.ToString().ToLowerInvariant()}_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.pdf";
        }
    }
}
