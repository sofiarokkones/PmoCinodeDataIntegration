using Newtonsoft.Json;

namespace BuildPmoList
{
    public class Team
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}