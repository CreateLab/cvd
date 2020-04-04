namespace Covid.Models
{
    public class AllCases
    {
        public int Cases { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public long Updated { get; set; }
        public int Active { get; set; }
        public int AffectedCountries { get; set; }
    }
}