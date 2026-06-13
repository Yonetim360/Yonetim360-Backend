using Microsoft.Extensions.Configuration;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class DocumentClassificationService : IDocumentClassificationService
    {
        private readonly Dictionary<DocumentFileType, List<string>> _keywords = new();

        public DocumentClassificationService(IConfiguration configuration)
        {
            var keywordsSection = configuration.GetSection("DocumentClassification:Keywords");

            foreach (DocumentFileType fileType in Enum.GetValues<DocumentFileType>())
            {
                var keywords = keywordsSection.GetSection(fileType.ToString()).Get<List<string>>();
                if (keywords is { Count: > 0 })
                {
                    _keywords[fileType] = keywords;
                }
            }
        }

        public DocumentFileType ClassifyDocument(string ocrText)
        {
            var (type, _) = ClassifyWithConfidence(ocrText);
            return type;
        }

        public (DocumentFileType type, double confidence) ClassifyWithConfidence(string ocrText)
        {
            if (string.IsNullOrWhiteSpace(ocrText))
            {
                return (DocumentFileType.BatchPdf, 0d);
            }

            var text = ocrText.ToLowerInvariant();
            var scores = new Dictionary<DocumentFileType, int>();

            foreach (var entry in _keywords)
            {
                scores[entry.Key] = entry.Value.Count(keyword => text.Contains(keyword.ToLowerInvariant()));
            }

            var bestMatch = scores.OrderByDescending(x => x.Value).FirstOrDefault();
            if (bestMatch.Value == 0 || !_keywords.ContainsKey(bestMatch.Key))
            {
                return (DocumentFileType.BatchPdf, 0d);
            }

            var confidence = (double)bestMatch.Value / _keywords[bestMatch.Key].Count;
            return (bestMatch.Key, Math.Min(confidence, 1d));
        }
    }
}
