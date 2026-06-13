using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.DTO.DocumentProcessing
{
    public class ExtractedDocumentDto
    {
        public Guid Id { get; set; }
        public DocumentFileType DocumentType { get; set; }
        public string ExtractedFileName { get; set; } = string.Empty;
        public string ExtractedPdfPath { get; set; } = string.Empty;
        public int StartPageNumber { get; set; }
        public int EndPageNumber { get; set; }
        public int PageCount { get; set; }
        public double AverageConfidence { get; set; }
        public bool IsSuccessfullyExtracted { get; set; }
        public string? ExtractionNotes { get; set; }
    }
}
