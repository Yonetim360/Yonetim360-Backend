using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.DTO.DocumentProcessing;

namespace Yonetim360Business.Services.Abstract
{
    public interface IPageGroupingService
    {
        List<PageGroupDto> GroupConsecutivePages(List<AnalyzedPage> analyzedPages, double confidenceThreshold = 0.3);
    }
}
