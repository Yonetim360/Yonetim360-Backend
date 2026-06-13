namespace Yonetim360.Entity.DocumentProcessing
{
    public class AnalyzedPage : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid ProcessedDocumentId { get; set; }
        public ProcessedDocument ProcessedDocument { get; set; } = null!;
        public int PageNumber { get; set; }
        public DocumentFileType DetectedDocumentType { get; set; } = DocumentFileType.BatchPdf;
        public double ClassificationConfidence { get; set; }
        public string? DetectedKeywords { get; set; }
        public int? DocumentGroupId { get; set; }
    }
}
