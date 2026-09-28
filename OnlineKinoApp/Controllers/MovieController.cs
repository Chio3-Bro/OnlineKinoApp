using Microsoft.AspNetCore.Mvc;
using OnlineKinoApp.Models;
using OnlineKinoApp.ViewModels;
using OnlineKinoApp.Services;

namespace OnlineKinoApp.Controllers
{
	public class MovieController : Controller
	{
		public static List<Movie> movies = new List<Movie>();

		private readonly BlobStorageService _blobStorageService;

		public MovieController(BlobStorageService blobStorageService)
		{
			_blobStorageService = blobStorageService;
		}

        public IActionResult Index()
        {
            Console.WriteLine($"MOVIES IN INDEX: {movies.Count}");
            foreach (var movie in movies)
            {
                if (!string.IsNullOrEmpty(movie.PosterBlobName))
                {
                    movie.PosterSasUrl =
                        _blobStorageService.GetSasUrl(movie.PosterBlobName);
                }
            }

            return View(movies);
        }

        [HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel model)
        {
            var poster = model.Poster;
            var video = model.Video;

            if (poster == null || poster.Length == 0)
            {
                ModelState.AddModelError("Poster", "Please select a poster.");
            }

            if (video == null || video.Length == 0)
            {
                ModelState.AddModelError("Video", "Please select a video.");
            }

            if (!ModelState.IsValid || poster == null || video == null)
            {
                return View(model);
            }

            string posterBlobName =
                await _blobStorageService.UploadFileAsync(poster);

            string videoBlobName =
                await _blobStorageService.UploadFileAsync(video);

            var movie = new Movie
            {
                Id = movies.Count + 1,
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Genre = model.Genre,
                PosterBlobName = posterBlobName,
                VideoBlobName = videoBlobName
            };

            movies.Add(movie);

            Console.WriteLine($"MOVIES AFTER ADD: {movies.Count}");

            return RedirectToAction("Index");
        }
    }
}
