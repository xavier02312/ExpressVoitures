namespace ExpressVoitures.Models.Service
{
    public interface IPathService
    {
        string GetUploadsPath(string? filename = null, bool withWebRootPath = true);
    }
}
