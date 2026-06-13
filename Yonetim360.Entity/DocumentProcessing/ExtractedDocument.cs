namespace Yonetim360.Entity.DocumentProcessing
{
    public class ExtractedDocument : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid ProcessedDocumentId { get; set; }
        public ProcessedDocument ProcessedDocument { get; set; } = null!;
        public DocumentFileType DocumentType { get; set; } = DocumentFileType.Unknown;
        public string ExtractedPdfPath { get; set; } = string.Empty;
        public string ExtractedFileName { get; set; } = string.Empty;
        public int StartPageNumber { get; set; }
        public int EndPageNumber { get; set; }
        public int PageCount { get; set; }
        public double AverageConfidence { get; set; }
        public string? CombinedKeywords { get; set; }
        public bool IsSuccessfullyExtracted { get; set; }
        public string? ExtractionNotes { get; set; }
    }
}
