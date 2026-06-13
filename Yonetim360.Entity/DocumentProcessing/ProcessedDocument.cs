namespace Yonetim360.Entity.DocumentProcessing
{
    public class ProcessedDocument : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid UploadedDocumentId { get; set; }
        public UploadedDocument UploadedDocument { get; set; } = null!;
        public string FilePath { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string? OcrTextSample { get; set; }
        public bool IsProcessed { get; set; }
        public string? Notes { get; set; }
        public DocumentFileType DetectedDocumentType { get; set; } = DocumentFileType.BatchPdf;
        public double ClassificationConfidence { get; set; }
        public string? DetectedKeywords { get; set; }
        public DocumentProcessStatus ProcessStatus { get; set; } = DocumentProcessStatus.Uploaded;

        public List<AnalyzedPage> AnalyzedPages { get; set; } = new();
        public List<ExtractedDocument> ExtractedDocuments { get; set; } = new();
    }
}
