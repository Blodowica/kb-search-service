using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieProductionCompany
    {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
    }
}
