using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildPmoList
{
    public class AvailabilityDataHandler : IJsonCiondeDataHandler
    {
        public string GetData(string responsFromHttpRequset)
        {
            var allAvailability = JsonConvert.DeserializeObject<List<Availability>>(responsFromHttpRequset);
            string availDataAllString = "";
            foreach (var availData in allAvailability)
            {
                var percentage = availData.PercentageTime;
                var startDate = availData.StartDate.Remove(10);
                var endDate = availData.EndDate.Remove(10);
                availDataAllString += percentage + " : " + startDate + " - " + endDate + "; ";
            }
            if (availDataAllString.Length == 0)
            {
                return "";
            }
            else
            {
                return availDataAllString.Remove(availDataAllString.Length - 2);

            }
        }
    }
}
