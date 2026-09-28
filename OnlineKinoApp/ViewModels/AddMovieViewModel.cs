using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace OnlineKinoApp.ViewModels
{
    public class AddMovieViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        public IFormFile? Poster { get; set; }
        public IFormFile? Video { get; set; }
    }
}
