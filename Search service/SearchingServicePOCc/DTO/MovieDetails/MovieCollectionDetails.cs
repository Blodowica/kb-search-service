using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieCollectionDetails
    {

        [JsonPropertyName("id")]
        public int CollectionId { get; set; }

        [JsonPropertyName("name")]
        public string? CollectionName { get; set; }
        [JsonPropertyName("poster_path")]
        public string? CollectionPoster { get;  set; }

   
    }
}
