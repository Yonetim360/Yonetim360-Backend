namespace Yonetim360.Entity.DocumentProcessing
{
    public class UploadedDocument : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string StoredPath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public Guid? OwnerId { get; set; }
        public string? SourceModule { get; set; }
        public bool IsProcessed { get; set; }

        public List<ProcessedDocument> ProcessedDocuments { get; set; } = new();
    }
}
