namespace SearchingService_POC_c_.DTO
{
    public class MovieCreditsResult
    {

        public string Director { get; set; } = "Not Available";
        public List<string> TopCast { get; set; } = new List<string>();

    }
}
