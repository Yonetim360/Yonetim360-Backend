using PDFtoImage;
using SkiaSharp;
using Tesseract;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class DocumentOcrService : IDocumentOcrService
    {
        public async Task<List<string>> ExtractTextPerPageAsync(string pdfPath)
        {
            var tessDataPath = GetTessDataPath();
            var ocrResults = new string[0];

            await using var fileStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read);
            var bitmaps = new List<SKBitmap>();

            await foreach (var image in Conversion.ToImagesAsync(fileStream))
            {
                bitmaps.Add(image);
            }

            ocrResults = new string[bitmaps.Count];

            var tasks = bitmaps.Select((bitmap, index) => Task.Run(() =>
            {
                try
                {
                    using var engine = new TesseractEngine(tessDataPath, "tur+eng", EngineMode.Default);
                    using var pix = PixConverter.ToPix(bitmap);
                    using var page = engine.Process(pix);
                    ocrResults[index] = page.GetText();
                }
                finally
                {
                    bitmap.Dispose();
                }
            }));

            await Task.WhenAll(tasks);

            return ocrResults.ToList();
        }

        private static string GetTessDataPath()
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(DocumentOcrService).Assembly.Location);
            if (!string.IsNullOrWhiteSpace(assemblyLocation))
            {
                var assemblyPath = Path.Combine(assemblyLocation, "tessdata");
                if (Directory.Exists(assemblyPath))
                {
                    return assemblyPath;
                }
            }

            var currentDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");
            if (Directory.Exists(currentDirectoryPath))
            {
                return currentDirectoryPath;
            }

            throw new DirectoryNotFoundException("Tessdata directory could not be found.");
        }

        private static class PixConverter
        {
            public static Pix ToPix(SKBitmap bitmap)
            {
                using var stream = new MemoryStream();
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                data.SaveTo(stream);
                stream.Position = 0;
                return Pix.LoadFromMemory(stream.ToArray());
            }
        }
    }
}
