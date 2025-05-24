using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchingService_POC_c_.DTO;
using SearchingService_POC_c_.Interfaces;
using SearchingService_POC_c_.Models;

namespace SearchingService_POC_c_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("MovieById")]
        public async Task<ActionResult<MovieDto>> GetMovieDetailsById(int movieId)
        {
            try
            {
                if (movieId < 0) { return BadRequest($"Movie id cannot be {movieId}"); }
                var movie = await _movieService.MovieDetailsById(movieId);
                if (movie == null) { return NotFound(); }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MovieBySearch")]
        public async Task<ActionResult<List<MovieDto>>> GetMovieBySearch(string query, int page)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) { return BadRequest("The search query is empty"); }
                if (page < 1) { page = 1; }

                var movie = await _movieService.GetMovieBySearch(query, page);
                if (movie == null) { return NotFound(); }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("MovieCredits")]
        public async Task<ActionResult<MovieCreditsResult>> GetMovieCredits(int movieId)
        {
            try
            {
                if (movieId < 1) { return BadRequest($"Movie id cannot be ${movieId}"); }
                var credits = await _movieService.GetMovieCredits(movieId);
                if (credits == null) { return NotFound(); };
                return Ok(credits);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MovieTrending")]
        public async Task<ActionResult<List<MovieDto>>> GetTrendingMovies(int pagination)
        {
            try
            {
                if (pagination < 1) { pagination = 1; }
                var trendingMovies = await _movieService.GetTrendingMovies(pagination);
                if (trendingMovies == null) { NotFound("There were not trending movies found"); }
                return Ok(trendingMovies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MovieSearchByFilter")]
        public async Task<ActionResult<List<MovieSearchResponse>>> GetMovieWithFilter(string? sortBy, string? genres, int? page, int? releaseYear, string? keywords, string? language)
        {
            try
            {
                var movies = await _movieService.GetMovieWithFilter(sortBy, genres, page, releaseYear, keywords, language);
                if (movies == null) { NotFound("No movie could be found with the current selected filters"); }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpGet("MovieRecommendations")]

        public async Task<ActionResult<List<MovieDto>>> GetMovieRecommendation(int movieId)
        {
            try
            {
                if (movieId < 0) { return BadRequest("There was no movie id provided"); }
                var movies = await _movieService.GetMovieRecommendations(movieId);
                if (movies == null) { NotFound("No movie could be found to recommend"); }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
