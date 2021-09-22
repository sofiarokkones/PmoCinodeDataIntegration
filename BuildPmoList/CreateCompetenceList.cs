using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace BuildPmoList
{
    public class CreateCompetenceList
    {

        public void GenerateList(string skillListConsultants, string totalSkillListCinode, string placeToSaveResult)
        {
            HashSet<string> ScannedRecords = new HashSet<string>();

            var temp = File.ReadAllLines(skillListConsultants);
            foreach (string line in temp)
            {
                var delimitedLine = line.Split(',');
                var delimitedLineSkills = delimitedLine[2].Split(';');
                foreach (string skill in delimitedLineSkills)
                {
                    ScannedRecords.Add(skill);
                }
            }


            // CSV WRITER
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csvWriter.Configuration.Delimiter = ";";

                int c = 0;
                var totalSkillList = File.ReadAllLines(totalSkillListCinode);
                foreach (string line in totalSkillList)
                {
                    var delimitedLine = line.Split(';');
                    if (c == 0 || (ScannedRecords.Contains(delimitedLine[0]) && int.Parse(delimitedLine[2]) > 4))
                    {
                        foreach (string data in delimitedLine)
                        {
                            csvWriter.WriteField(data);
                        }
                        csvWriter.NextRecord();
                    }
                    c++;
                }


                // SAVE DATA FROM CSV-FILE TO A .TXT-FILE --------------
                writer.Flush();
                var resultOfCsv = Encoding.UTF8.GetString(mem.ToArray());
                var s = Regex.Replace(resultOfCsv, "\"", string.Empty);
                File.WriteAllText(placeToSaveResult, s);
            }
        }
    }
}







