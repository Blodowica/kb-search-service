using SearchingService_POC_c_.Models;

namespace SearchingService_POC_c_.DTO
{
    public class MovieCreditsResult
    {

        public CrewMember? Director { get; set; } = new CrewMember();
        public List<CastMember>? TopCast { get; set; } = new List<CastMember>();

        public CrewMember? Author { get; set; } = new CrewMember();

        public CrewMember? Producer { get; set; } = new CrewMember();

    }
}
