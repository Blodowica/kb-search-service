using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieKeywords
    {

        [JsonPropertyName("id")]
        public required int KeywordId { get; set; }
        
        [JsonPropertyName("name")]
        public required string KeywordName { get; set; }

    }
}
