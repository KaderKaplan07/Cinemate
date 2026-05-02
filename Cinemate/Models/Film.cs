namespace Cinemate.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? PosterPath { get; set; }
        public string? Overview { get; set; }
        public double Rating { get; set; }

        public string Poster =>
            string.IsNullOrEmpty(PosterPath)
                ? "/images/no-poster.jpg"
                : "https://image.tmdb.org/t/p/w500" + PosterPath;
    }
}
