using Newtonsoft.Json;

namespace BuildPmoList
{
    public class Availability
    {
        [JsonProperty("availability")]
        public string PercentageTime { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }
    }
}