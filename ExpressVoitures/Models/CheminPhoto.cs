namespace ExpressVoitures.Models
{
    public class CheminPhoto
    {
        //"Path" correspond à la constante définit dans appsettings.json
        public const string chemin = "Path";

        public string ImagesVoitures { get; set; } = string.Empty;
    }
}
