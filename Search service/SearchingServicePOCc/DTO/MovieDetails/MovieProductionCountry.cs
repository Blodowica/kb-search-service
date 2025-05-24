using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.DTO.MovieDetails
{
    public class MovieProductionCountry
    {


        [JsonPropertyName("iso_3166_1")]
        public string CountryId { get; set; }

        [JsonPropertyName("name")]
        public string CountryName { get; set; }


     
    }
}
