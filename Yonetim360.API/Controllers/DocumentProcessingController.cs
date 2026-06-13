using Microsoft.AspNetCore.Mvc;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.CQRS.DocumentProcessing.Commands.CreateUploadedDocument;
using Yonetim360Business.CQRS.DocumentProcessing.Commands.DeleteUploadedDocument;
using Yonetim360Business.CQRS.DocumentProcessing.Commands.ProcessUploadedDocument;

namespace Yonetim360.API.Controllers
{
    [Route("api/document-processing")]
    [ApiController]
    public class DocumentProcessingController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentProcessingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] CreateUploadedDocumentCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("process")]
        public async Task<IActionResult> Process([FromBody] ProcessUploadedDocumentCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{uploadedDocumentId:guid}")]
        public async Task<IActionResult> Delete(Guid uploadedDocumentId)
        {
            var result = await Mediator.Send(new DeleteUploadedDocumentCommand
            {
                UploadedDocumentId = uploadedDocumentId
            });

            return Ok(result);
        }

        [HttpGet("uploaded/{uploadedDocumentId:guid}/download")]
        public async Task<IActionResult> DownloadUploaded(Guid uploadedDocumentId)
        {
            var repository = _unitOfWork.GetRepository<UploadedDocument>();
            var document = await repository.GetFirstOrDefaultAsync(x => x.Id == uploadedDocumentId && !x.IsDeleted, tracked: false);

            if (document == null || !System.IO.File.Exists(document.StoredPath))
            {
                return NotFound();
            }

            return PhysicalFile(
                document.StoredPath,
                string.IsNullOrWhiteSpace(document.MimeType) ? "application/pdf" : document.MimeType,
                document.OriginalFileName);
        }

        [HttpGet("extracted/{extractedDocumentId:guid}/download")]
        public async Task<IActionResult> DownloadExtracted(Guid extractedDocumentId)
        {
            var repository = _unitOfWork.GetRepository<ExtractedDocument>();
            var document = await repository.GetFirstOrDefaultAsync(x => x.Id == extractedDocumentId && !x.IsDeleted, tracked: false);

            if (document == null || !System.IO.File.Exists(document.ExtractedPdfPath))
            {
                return NotFound();
            }

            return PhysicalFile(
                document.ExtractedPdfPath,
                "application/pdf",
                document.ExtractedFileName);
        }
    }
}
