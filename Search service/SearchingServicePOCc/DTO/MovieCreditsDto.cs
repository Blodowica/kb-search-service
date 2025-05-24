using System.Text.Json.Serialization;

namespace SearchingService_POC_c_.Models
{
    public class MovieCreditsDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; } 

        [JsonPropertyName("cast")]
        public List<CastMember>? Cast { get; set; } = new List<CastMember>();

        [JsonPropertyName("crew")]
        public List<CrewMember>? Crew { get; set; } = new List<CrewMember>();

    }



    public class CastMember
    {
        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("name")]
        public string? ActorName { get; set; } = string.Empty;


        [JsonPropertyName("profile_path")]
        public string? ProfileImage { get; set; } = string.Empty;



        [JsonPropertyName("character")]
        public string? CharacterName { get; set; } = string.Empty;
    }


    public class CrewMember
    {
        [JsonPropertyName("job")]
        public string? Job { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = string.Empty;


        [JsonPropertyName("profile_path")]
        public string? ProfileImage { get; set; } = string.Empty;
    }
}
