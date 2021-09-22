using System;
using System.IO;

namespace BuildPmoList
{
    public static class Program
    {

        static async System.Threading.Tasks.Task Main(string[] args)
        {

            // EDIT FOLLOWING VARIABLES IN THIS SECTION ---------------------------------------------------------------------------

            // Paste your access ID and secret from Cinode here
            var accessId = "<Åtkomstid>";
            var accessSecret = "<Åtkomsthemlighet>";


            // Select end date for wanted availability time
            string today = string.Format("{0:yyyy-MM-dd}", DateTime.Now); // Start date: today
            string untilDate = "2021-12-30"; // End date: --> Edit as you prefer

            // Select where you want to save the resulting text file and name it
            // for ex. = @"/Users/your_account/OneDrive - Forefront Consulting Group/CiondeApiData.txt"
            // where CiondeApiData is the selected file name
            var placeToSaveResult = @"<Your wanted palce to save it to and its name>";

            // --------------------------------------------------------------------------------------------------------------------
            
             BuildList list = new BuildList();
             await list.BuildPMOList(accessId, accessSecret, placeToSaveResult, today, untilDate);

             // GENERATE SKILL LIST
            // Places where to get already created lists
            var skillListConsultants = @"/placeToSave/file.csv";
            var totalSkillListCinode = @"/placeToSave/file.csv";

            // Place where to save the skill list result
            var placeToSaveSkillListResult = @"/placeToSave/file.csv";

            CreateCompetenceList competenceList = new CreateCompetenceList();
            competenceList.GenerateList(skillListConsultants, totalSkillListCinode, placeToSaveSkillListResult);

            CsvToJson csvJson = new CsvToJson();
            string jsonResult = csvJson.ConvertCsvFileToJsonObject(placeToSaveResult);
            File.WriteAllText(@"/placeToSave/file.txt", jsonResult);
        }
    }

}

