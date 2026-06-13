using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class PageGroupingService : IPageGroupingService
    {
        public List<PageGroupDto> GroupConsecutivePages(List<AnalyzedPage> analyzedPages, double confidenceThreshold = 0.3)
        {
            var groups = new List<PageGroupDto>();
            if (analyzedPages.Count == 0)
            {
                return groups;
            }

            var sortedPages = analyzedPages.OrderBy(p => p.PageNumber).ToList();
            PageGroupDto? currentGroup = null;

            foreach (var page in sortedPages)
            {
                var isConfident = page.ClassificationConfidence >= confidenceThreshold;
                var isClassified = page.DetectedDocumentType != DocumentFileType.BatchPdf;

                if (isConfident && isClassified)
                {
                    if (currentGroup != null)
                    {
                        groups.Add(currentGroup);
                    }

                    currentGroup = new PageGroupDto
                    {
                        StartPage = page.PageNumber,
                        EndPage = page.PageNumber,
                        DocumentType = page.DetectedDocumentType,
                        AverageConfidence = page.ClassificationConfidence,
                        Keywords = page.DetectedKeywords,
                        Pages = new List<AnalyzedPage> { page }
                    };
                }
                else
                {
                    if (currentGroup == null)
                    {
                        currentGroup = new PageGroupDto
                        {
                            StartPage = page.PageNumber,
                            EndPage = page.PageNumber,
                            DocumentType = DocumentFileType.BatchPdf,
                            AverageConfidence = page.ClassificationConfidence,
                            Keywords = page.DetectedKeywords,
                            Pages = new List<AnalyzedPage> { page }
                        };
                    }
                    else
                    {
                        currentGroup.EndPage = page.PageNumber;
                        currentGroup.AverageConfidence = (currentGroup.AverageConfidence + page.ClassificationConfidence) / 2d;
                        currentGroup.Keywords = string.Join(", ", new[] { currentGroup.Keywords, page.DetectedKeywords }.Where(x => !string.IsNullOrWhiteSpace(x)));
                        currentGroup.Pages.Add(page);
                    }
                }
            }

            if (currentGroup != null)
            {
                groups.Add(currentGroup);
            }

            return groups;
        }
    }
}
