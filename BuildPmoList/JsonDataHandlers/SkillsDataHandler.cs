using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildPmoList
{
    public class SkillsDataHandler : IJsonCiondeDataHandler
    {

        public string GetData(string responsFromHttpRequset)
        {
            var allSkills = JsonConvert.DeserializeObject<List<Skills>>(responsFromHttpRequset);
            string allSkillsString = "";
            foreach (var skillData in allSkills)
            {
                var level = skillData.Skill.LevelOfSKill;
                if (level > 2 || level < 6)
                {
                    var userSkill = skillData.Skill.NameOfSkill;
                    allSkillsString += userSkill + ";";
                }
            }

            if (allSkillsString == "")
            {
                return "";
               
            }
            else
            {
                return allSkillsString.Remove(allSkillsString.Length - 1);
          
            }
        }
    }
}
