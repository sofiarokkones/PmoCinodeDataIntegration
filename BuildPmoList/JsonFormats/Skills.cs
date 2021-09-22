using Newtonsoft.Json;

namespace BuildPmoList
{
    public class Skills
    {
        [JsonProperty("keyword")]
        public Skill Skill { get; set; }
    }

    public class Skill
    {
        [JsonProperty("masterSynonym")]
        public string NameOfSkill { get; set; }

        [JsonProperty("level")]
        public int LevelOfSKill { get; set; }
    }
}