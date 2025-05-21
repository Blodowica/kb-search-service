using System.Text.Json.Serialization;
using SearchingService_POC_c_.Models;
namespace SearchingService_POC_c_.DTO
{
  
        public class MovieSearchResponse
        {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<MovieDto> Results { get; set; } = new List<MovieDto>();

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }

    


    }
