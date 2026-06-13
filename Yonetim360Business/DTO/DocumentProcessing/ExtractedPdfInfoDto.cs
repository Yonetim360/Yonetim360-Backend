using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.DTO.DocumentProcessing
{
    public class ExtractedPdfInfoDto
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DocumentFileType DocumentType { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }
        public double AverageConfidence { get; set; }
        public string? CombinedKeywords { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
