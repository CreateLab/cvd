using Avalonia.Media.Imaging;

namespace Covid.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string ISO3 { get; set; }
        public int Cases { get; set; }
        public int TodayCases { get; set; }
        public int Deaths { get; set; }
        public int TodayDeaths { get; set; }
        public int Recovered { get; set; }
        public double? DeathsPerOneMillion { get; set; }
        public Bitmap FlagImage { get; set; }
    }
}