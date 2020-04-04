namespace Covid.Models.SupportModels
{
    public class CountryInfo
    {
        public int? Id { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public object Lat { get; set; }
        public object @Long { get; set; }
        public string Flag { get; set; }
    }
}