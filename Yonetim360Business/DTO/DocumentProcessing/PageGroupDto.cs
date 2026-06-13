using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.DTO.DocumentProcessing
{
    public class PageGroupDto
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public DocumentFileType DocumentType { get; set; }
        public double AverageConfidence { get; set; }
        public string? Keywords { get; set; }
        public List<AnalyzedPage> Pages { get; set; } = new();
    }
}
