using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.DTO.DocumentProcessing
{
    public class ProcessedDocumentResultDto
    {
        public Guid ProcessedDocumentId { get; set; }
        public Guid UploadedDocumentId { get; set; }
        public int PageCount { get; set; }
        public DocumentFileType DetectedDocumentType { get; set; }
        public double ClassificationConfidence { get; set; }
        public List<ExtractedDocumentDto> ExtractedDocuments { get; set; } = new();
    }
}
