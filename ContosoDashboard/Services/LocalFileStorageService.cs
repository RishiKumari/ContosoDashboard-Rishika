namespace ContosoDashboard.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService()
    {
        var root = Path.Combine(AppContext.BaseDirectory, "AppData", "uploads");
        Directory.CreateDirectory(root);
        _basePath = root;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, int userId, int? projectId = null)
    {
        var safeFileName = Path.GetFileName(fileName);
        var extension = Path.GetExtension(safeFileName);
        var uniqueName = $"{Guid.NewGuid():N}{extension}";
        var folderName = projectId.HasValue ? projectId.Value.ToString() : "personal";
        var directory = Path.Combine(_basePath, userId.ToString(), folderName);

        Directory.CreateDirectory(directory);

        var fullPath = Path.Combine(directory, uniqueName);
        await using var destination = File.Create(fullPath);
        await fileStream.CopyToAsync(destination);

        return Path.Combine(userId.ToString(), folderName, uniqueName).Replace('\\', '/');
    }

    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public async Task<Stream> DownloadAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath.Replace('/', Path.DirectorySeparatorChar));
        return await Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public Task<string> GetUrlAsync(string filePath, TimeSpan expiration)
    {
        return Task.FromResult($"/api/documents/download?path={Uri.EscapeDataString(filePath)}");
    }
}
