using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.Models
{
    public class MovieDto
    {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            // Map "title" from JSON to Title
            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;




            [JsonPropertyName("overview")]
            public string Description { get; set; } = string.Empty;

            [JsonPropertyName("poster_path")]
            public string PosterURL { get; set; } = string.Empty;


            [JsonPropertyName("original_language")]
            public string OriginalLanguage { get; set; } = string.Empty;


            [JsonPropertyName("release_date")]
            public string ReleaseDate { get; set; } = string.Empty;

            [JsonPropertyName("runtime")]
            public int RunTime { get; set; }


            [JsonPropertyName("genres")]

            public List<Object> Genres { get; set; } = new List<Object>();

            [JsonPropertyName("genre_ids")]
            public List<int>? GenreIds { get; set; } = new List<int>();

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("tagline")]
            public string Tagline { get; set; } = string.Empty;

            [JsonPropertyName("origin_country")]
            public List<string>? OriginCountries { get; set; }

            [JsonPropertyName("production_companies")]
            public List<ProductionCompany>? productionCompanies { get; set; }
    }

    public class ProductionCompany
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
