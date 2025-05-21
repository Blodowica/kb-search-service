using SearchingService_POC_c_.DTO;
using SearchingService_POC_c_.Models;

namespace SearchingService_POC_c_.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDto?> MovieDetailsById(int movieId);

        Task<MovieCreditsResult> GetMovieCredits(int movieId);

        Task<List<MovieDto>> GetMovieBySearch(string query, int page);

        Task<List<MovieDto>> GetTrendingMovies(int pagination);

        Task<List<MovieDto>> GetMovieWithFilter(string? sortBy, string? genres, int? page, int? releaseYear, string? keywords, string? language);
    }
}
