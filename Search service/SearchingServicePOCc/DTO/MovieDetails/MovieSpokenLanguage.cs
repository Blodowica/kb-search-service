using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieSpokenLanguage
    {

    [JsonPropertyName("english_name")]
    public string? EnglishName { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string? LanguageId { get; set; }
       
    [JsonPropertyName("name")]
   public string? LanguageName { get; set; }

    }
}
