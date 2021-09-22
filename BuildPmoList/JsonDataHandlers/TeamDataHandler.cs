using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildPmoList
{
    public class TeamDataHandler : IJsonCiondeDataHandler
    {
        public string GetData(string responsFromHttpRequset)
        {
            var team = JsonConvert.DeserializeObject<List<Team>>(responsFromHttpRequset);
            var teamName = "";
            if (team.Count == 0)
            {
                teamName = "No team";
            }
            else
            {
                teamName = team[0].Name;
            }

            // Check if this user is in a cunsultant team
            if (teamName.Contains("Ledning") || teamName.Contains("President Office") || teamName.Contains("Övrigt") || teamName.Contains("Frontshore"))
            {
                teamName = "";
            }
            return teamName;
        }
    }
}
