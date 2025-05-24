using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RestSharp;
using SearchingService_POC_c_.DTO;
using SearchingService_POC_c_.Interfaces;
using SearchingService_POC_c_.Models;

namespace SearchingService_POC_c_.Services
{
    public class MovieService : IMovieService
    {
        private readonly RestClient _client;
        private readonly TMDbSettings _settings;
        private readonly Dictionary<string, string> _genreMap;

        public MovieService(IOptions<TMDbSettings> settings)
        {
            _settings = settings.Value;

            var options = new RestClientOptions(_settings.BaseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(180)
            };

            _client = new RestClient(options);

            // Load genre mappings from JSON file
            var genreJsonPath = Path.Combine(AppContext.BaseDirectory, "genres.json");
            if (File.Exists(genreJsonPath))
            {
                var json = File.ReadAllText(genreJsonPath);
                _genreMap = JsonSerializer.Deserialize<Dictionary<string, string>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new Dictionary<string, string>();
            }
            else
            {
                _genreMap = new Dictionary<string, string>();
            }

        }


        public async Task<List<MovieDto>> GetMovieWithFilter(string? sortBy, string? genres, int? page, int? releaseYear, string? keywords, string? language)
        {
            var request = new RestRequest("discover/movie", Method.Get);

            // Set default page number
            int pageNumber = (page.HasValue && page.Value > 0) ? page.Value : 1;
            request.AddQueryParameter("page", pageNumber.ToString());

            // Add release year if valid
            if (releaseYear.HasValue && releaseYear.Value > 0)
                request.AddQueryParameter("primary_release_year", releaseYear.Value.ToString());

            // Set language, default to "en-US" if not provided
            request.AddQueryParameter("language", string.IsNullOrWhiteSpace(language) ? "en-US" : language);

            // Add keywords if provided
            if (!string.IsNullOrWhiteSpace(keywords))
                request.AddQueryParameter("with_keywords", keywords);

            // Add sort parameter if provided
            if (!string.IsNullOrWhiteSpace(sortBy))
                request.AddQueryParameter("sort_by", sortBy);

            // Set headers
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");

            // Process genres
            if (!string.IsNullOrWhiteSpace(genres))
            {
                var genreNames = genres.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var genreIds = new List<string>();

                foreach (var name in genreNames)
                {
                    var normalized = name.Trim().ToLowerInvariant();
                    if (_genreMap.TryGetValue(normalized, out var id))
                    {
                        genreIds.Add(id);
                    }
                    else
                    {
                        // Optionally, handle unknown genres
                        // e.g., log a warning or ignore
                    }
                }

                if (genreIds.Any())
                {
                    request.AddQueryParameter("with_genres", string.Join(",", genreIds));
                }
            }

            // Execute the request
            var response = await _client.GetAsync<MovieSearchResponse>(request);

            if (response?.Results == null)
            {
                return null;
            }

            // Update poster URLs
            foreach (var movie in response.Results.Where(m => m != null))
            {
                if (!string.IsNullOrWhiteSpace(movie.PosterURL))
                {
                    movie.PosterURL = $"{_settings.BaseImageUrl}{movie.PosterURL}";
                }
            }

            return response.Results;
        }


        public async Task<List<MovieDto>> GetMovieBySearch(string query, int page)
        {
            // Build the request: base URL must be set in appsettings (e.g., "https://api.themoviedb.org/3/")
            var request = new RestRequest($"search/movie", Method.Get);
            request.AddQueryParameter("query", query);
            request.AddQueryParameter("language", "en-US");
            request.AddQueryParameter("include_adult", "false");
            request.AddQueryParameter("page", page);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");


            // Use GetAsync<MovieDto> to get the deserialized object directly
            var response = await _client.GetAsync<MovieSearchResponse>(request);

            if (response == null || response.Results == null) { return null; }
            foreach (var movie in response.Results.Where(m => m != null))
            {
                if (!string.IsNullOrWhiteSpace(movie.PosterURL))
                {
                    movie.PosterURL = $"{_settings.BaseImageUrl}{movie.PosterURL}";
                }
            }

            return response.Results;

        }

        public async Task<MovieCreditsResult> GetMovieCredits(int movieId)
        {
            var request = new RestRequest($"movie/{movieId}/credits", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");

            // Use GetAsync<MovieDto> to get the deserialized object directly
            var movieCredits = await _client.GetAsync<MovieCreditsDto>(request);
            if (movieCredits == null) return null;

            // Get director name from crew (looking for a crew member with job "Director")
            CrewMember? director = movieCredits?.Crew?.FirstOrDefault(c => c.Job == "Director");
            CrewMember? author = movieCredits?.Crew?.FirstOrDefault(c => c.Job == "Author");
            CrewMember? producer = movieCredits?.Crew?.FirstOrDefault(c => c.Job == "Producer");

            // Get top 3 cast members, ordered by the "order" property
            var topCast = movieCredits?.Cast?.OrderBy(c => c.Order).Take(3).ToList();

           //convert partial image url to full url
            if(director != null) director.ProfileImage = $"{_settings.BaseImageUrl}{director.ProfileImage}";
            if (author != null) author.ProfileImage = $"{_settings.BaseImageUrl}{author.ProfileImage}";
            if (producer != null) producer.ProfileImage = $"{_settings.BaseImageUrl}{producer.ProfileImage}";

            if (topCast != null)
            {

            foreach (CastMember cast in topCast)
            {
                    cast.ProfileImage = $"{_settings.BaseImageUrl}{cast.ProfileImage}";
            }

            }
            return new MovieCreditsResult
            {
                Director = director,
                TopCast = (List<CastMember>?)topCast,
                Producer = producer,
                Author = author
            }
           ;
        }

        public async Task<List<MovieDto>> GetTrendingMovies(int pagination)
        {
            var request = new RestRequest($"trending/movie/week", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");

            // Use GetAsync<MovieDto> to get the deserialized object directly
            var movies = await _client.GetAsync<MovieSearchResponse>(request);
            if (movies == null || movies.Results == null) { return null; }

            foreach (var movie in movies.Results.Where(m => m != null))
            {
                if (!string.IsNullOrWhiteSpace(movie.PosterURL))
                {
                    movie.PosterURL = $"{_settings.BaseImageUrl}{movie.PosterURL}";
                    movie.GenreIds = movie.GenreIds?.ToList();
                }
            }

            return movies.Results;
        }


        public async Task<MovieDto?> MovieDetailsById(int movieId)
        {
            var request = new RestRequest($"movie/{movieId}?append_to_response=keywords", Method.Get);
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");

            // Use GetAsync<MovieDto> to get the deserialized object directly
            var dto = await _client.GetAsync<MovieDto>(request);


            if (dto == null)
                return null;

            // Optionally, adjust the PosterURL if needed:
            if (!string.IsNullOrWhiteSpace(dto.PosterURL))
            {
                dto.PosterURL = $"{_settings.BaseImageUrl}" + dto.PosterURL;

            }
            if (!string.IsNullOrWhiteSpace(dto.BackdropURL))
            {
                dto.BackdropURL= $"{_settings.BaseImageUrl}" + dto.BackdropURL;
            }
            if (!string.IsNullOrEmpty(dto.MovieCollectionDetails?.CollectionPoster))
            {
                dto.MovieCollectionDetails.CollectionPoster = $"{_settings.BaseImageUrl}" + dto.MovieCollectionDetails.CollectionPoster;
            }
            return dto;
        }

        public async Task<List<MovieDto>> GetMovieRecommendations(int movieId)
        {
            try
            {
                var request = new RestRequest($"movie/{movieId}/recommendations");

                AddRestRequestParameters(request);

                var response = await _client.GetAsync<MovieSearchResponse>(request);

                if (response == null || response.Results == null)
                {
                    return null;
                }

                foreach (var movie in response.Results.Where(m => m != null))
                {
                    if (!string.IsNullOrWhiteSpace(movie.PosterURL))
                    {
                        movie.PosterURL = $"{_settings.BaseImageUrl}{movie.PosterURL}";
                        movie.GenreIds = movie.GenreIds?.ToList();
                    }

                }
                return response.Results;



            }
            catch (Exception)
            {

                throw;
            }

        }

        private void AddRestRequestParameters(RestRequest request)
        {
            request.AddQueryParameter("language", "en-US");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_settings.BearerToken}");
        }




    }
}
