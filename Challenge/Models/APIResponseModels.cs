using System.Collections.Generic;

namespace Challenge.Models
{
    public class LatestRateResponse
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class HistoricalRateResponse
    {
        public string Base { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    }
}
