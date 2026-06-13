using Yonetim360.Entity.DocumentProcessing;

namespace Yonetim360Business.Services.Abstract
{
    public interface IDocumentClassificationService
    {
        DocumentFileType ClassifyDocument(string ocrText);
        (DocumentFileType type, double confidence) ClassifyWithConfidence(string ocrText);
    }
}
