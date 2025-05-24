using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieKeywordWrapper
    {

        [JsonPropertyName("keywords")]

        public List<MovieKeywords>? MovieKeywords { get; set; }

    }
}
