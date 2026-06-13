namespace Yonetim360Business.Services.Abstract
{
    public interface IDocumentOcrService
    {
        Task<List<string>> ExtractTextPerPageAsync(string pdfPath);
    }
}
