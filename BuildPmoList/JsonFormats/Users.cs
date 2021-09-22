using Newtonsoft.Json;

namespace BuildPmoList
{
    public class User
    {
        [JsonProperty("companyUserEmail")]
        public string CompanyUserEmail { get; set; }

        [JsonProperty("companyUserId")]
        public string CompanyUserId { get; set; }

        [JsonProperty("seoId")]
        public string SeoId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

}