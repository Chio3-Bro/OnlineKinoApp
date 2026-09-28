namespace OnlineKinoApp.Models
{
    public class Movie
    {
        public int Id {  get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string? PosterBlobName { get; set; }
        public string? VideoBlobName { get; set; }

        public string? PosterSasUrl {  get; set; }
        public string? VideoSasUrl { get; set; }
    }
}
