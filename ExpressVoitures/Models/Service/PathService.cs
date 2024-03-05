namespace ExpressVoitures.Models.Service
{
    public class PathService : IPathService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environement;

        public PathService(IConfiguration configuration, IWebHostEnvironment environement)
        {
            _configuration = configuration;
            _environement = environement;
        }

        public string GetUploadsPath(string? filename = null, bool withWebRootPath = true)
        {
            var cheminPhoto = new CheminPhoto();

            _configuration.GetSection(CheminPhoto.chemin).Bind(cheminPhoto);

            var uploadPath = cheminPhoto.ImagesVoitures;

            if (filename != null)
                uploadPath = Path.Combine(uploadPath, filename);

            return withWebRootPath ? Path.Combine(_environement.WebRootPath, uploadPath) : uploadPath;
        }
    }
}
