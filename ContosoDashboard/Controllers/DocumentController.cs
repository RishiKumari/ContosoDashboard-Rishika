using System.Security.Claims;
using ContosoDashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoDashboard.Controllers;

[ApiController]
[Route("api/documents")]
[Authorize]
[RequestSizeLimit(MaxFileSizeBytes + (1024 * 1024))]
public class DocumentController : ControllerBase
{
    private const long MaxFileSizeBytes = 25 * 1024 * 1024;
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile? file,
        [FromForm] string title,
        [FromForm] string? description,
        [FromForm] string category,
        [FromForm] int? projectId)
    {
        try
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Unauthorized(new { message = "Authentication is required to upload documents." });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "A file is required for upload." });
            }

            if (file.Length > MaxFileSizeBytes)
            {
                return BadRequest(new { message = "The uploaded file exceeds the 25 MB size limit." });
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest(new { message = "Document title is required." });
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(new { message = "Document category is required." });
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Unable to identify the authenticated user." });
            }

            using var stream = file.OpenReadStream();
            var document = await _documentService.UploadDocumentAsync(
                userId.Value,
                title,
                description,
                category,
                projectId,
                stream,
                file.FileName,
                file.ContentType);

            return Ok(new
            {
                message = "Document uploaded successfully.",
                documentId = document.DocumentId,
                title = document.Title,
                category = document.Category,
                fileName = document.FileName,
                fileSizeBytes = document.FileSizeBytes,
                uploadedByUserId = document.UploadedByUserId,
                projectId = document.ProjectId,
                uploadedAt = document.CreatedDate
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            var message = ex.Message;

            if (message.Contains("Unsupported file type", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("size limit", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message });
            }

            if (message.Contains("required", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while uploading the document." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error uploading document for user {UserId}.", GetCurrentUserId());
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred while uploading the document." });
        }
    }

    private int? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(value, out var userId))
        {
            return userId;
        }

        return null;
    }
}
