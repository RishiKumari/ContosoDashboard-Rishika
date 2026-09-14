using Microsoft.EntityFrameworkCore;
using ContosoDashboard.Data;
using ContosoDashboard.Models;

namespace ContosoDashboard.Services;

public interface IDocumentService
{
    Task<Document> UploadDocumentAsync(int userId, string title, string? description, string category, int? projectId, Stream fileStream, string fileName, string contentType, bool allowProjectUpload = false);
    Task<List<Document>> GetUserDocumentsAsync(int userId);
    Task<List<Document>> GetProjectDocumentsAsync(int projectId, int requestingUserId);
}

public class DocumentService : IDocumentService
{
    private const long MaxFileSizeBytes = 25 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".jpg", ".jpeg", ".png"
    };

    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public DocumentService(ApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task<Document> UploadDocumentAsync(int userId, string title, string? description, string category, int? projectId, Stream fileStream, string fileName, string contentType, bool allowProjectUpload = false)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("Document title is required.");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidOperationException("Document category is required.");

        if (fileStream == null || fileStream.Length == 0)
            throw new InvalidOperationException("File content is required.");

        if (fileStream.Length > MaxFileSizeBytes)
            throw new InvalidOperationException("File exceeds the 25 MB size limit.");

        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("Unsupported file type.");

        if (projectId.HasValue)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId.Value);

            if (project == null)
                throw new InvalidOperationException("Project not found.");

            var allowed = project.ProjectManagerId == userId || project.ProjectMembers.Any(pm => pm.UserId == userId);
            if (!allowed && !allowProjectUpload)
                throw new UnauthorizedAccessException("You do not have permission to upload documents to this project.");
        }

        var safeStoredPath = await _fileStorageService.UploadAsync(fileStream, fileName, contentType, userId, projectId);

        var document = new Document
        {
            Title = title,
            Description = description,
            Category = category,
            ProjectId = projectId,
            UploadedByUserId = userId,
            FileName = Path.GetFileName(fileName),
            FileType = contentType.Length > 255 ? contentType.Substring(0, 255) : contentType,
            FileSizeBytes = fileStream.Length,
            FilePath = safeStoredPath,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return document;
    }

    public async Task<List<Document>> GetUserDocumentsAsync(int userId)
    {
        return await _context.Documents
            .Where(d => d.UploadedByUserId == userId)
            .OrderByDescending(d => d.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<Document>> GetProjectDocumentsAsync(int projectId, int requestingUserId)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null)
            return new List<Document>();

        var isAuthorized = project.ProjectManagerId == requestingUserId || project.ProjectMembers.Any(pm => pm.UserId == requestingUserId);
        if (!isAuthorized)
            return new List<Document>();

        return await _context.Documents
            .Where(d => d.ProjectId == projectId)
            .OrderByDescending(d => d.CreatedDate)
            .ToListAsync();
    }
}
