using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.DTO.DocumentProcessing
{
    public class UploadedDocumentDto
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string StoredPath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }
        public DocumentProcessStatus ProcessStatus { get; set; }
    }
}
