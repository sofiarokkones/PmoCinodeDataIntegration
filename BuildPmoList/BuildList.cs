using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BuildPmoList
{
    public class BuildList
    {
        public async System.Threading.Tasks.Task BuildPMOList(string accessId, string accessSecret, string placeToSaveResult, string today, string untilDate)
        {

            // Authorization by accessId and accessSecret to obtain access token
            HttpClient httpClient_token = new HttpClient();
            IJsonCiondeDataHandler dataHandler;

            var basicParameter = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accessId}:{accessSecret}"));
            httpClient_token.DefaultRequestHeaders.Add("Authorization", "Basic " + basicParameter);

            var basicResponse = await httpClient_token.GetAsync("https://api.cinode.com/token");

            if (basicResponse.IsSuccessStatusCode)
            {
                var basicResult = await basicResponse.Content.ReadAsStringAsync();

                var definition = new { access_token = "", refresh_token = "" };
                var tokenResponse = JsonConvert.DeserializeAnonymousType(basicResult, definition);

                HttpClient httpClient_data = new HttpClient();
                httpClient_data.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenResponse.access_token);
                httpClient_data.DefaultRequestHeaders.Accept.Clear();
                httpClient_data.DefaultRequestHeaders.Add("ContentType", "application/json");

                // GET CINODE DATA - start with all users, extended version
                var response = await httpClient_data.GetAsync("https://api.cinode.com/v0.1/companies/140/users/extended");

                if (response.IsSuccessStatusCode)
                {
                    var result_0 = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<User>>(result_0);


                    // Constructs the csv file writer to save the data
                    using (var mem = new MemoryStream())
                    using (var writer = new StreamWriter(mem))
                    using (var csvWriter = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                    {

                        csvWriter.Configuration.Delimiter = ",";

                        csvWriter.WriteField("Email");
                        csvWriter.WriteField("Team");
                        csvWriter.WriteField("UserSkills");
                        csvWriter.WriteField("Availability");
                        csvWriter.WriteField("CinodeProfile");
                        csvWriter.WriteField("FirstName");
                        csvWriter.WriteField("LastName");
                        csvWriter.WriteField("FullName");
                        csvWriter.NextRecord();

                        bool isConsultant = false;

                        foreach (var user in users)
                        {

                            // COLLECT DATA FOR EACH COMPANY USER BY ID --------------
                            var companyUserId = user.CompanyUserId;

                            // Get Team for this UserId
                            var response_team = await httpClient_data.GetAsync("https://api.cinode.com/v0.1/companies/140/users/" + companyUserId + " /teams");
                            if (response_team.IsSuccessStatusCode)
                            {
                                var respons_team_0 = await response_team.Content.ReadAsStringAsync();
                                dataHandler = new TeamDataHandler();
                                var result = dataHandler.GetData(respons_team_0);

                                if (result == "")
                                {
                                    isConsultant = false;
                                }
                                else
                                {
                                    isConsultant = true;
                                    csvWriter.WriteField(user.CompanyUserEmail);
                                    csvWriter.WriteField(result);
                                }
                            }


                            if (isConsultant)
                            {
                                //Get skills and level for this UserId
                                var response_skills = await httpClient_data.GetAsync("https://api.cinode.com/v0.1/companies/140/users/" + companyUserId + "/skills");
                                if (response_skills.IsSuccessStatusCode)
                                {
                                    var respons_skills_0 = await response_skills.Content.ReadAsStringAsync();
                                    //respons_skills_0 = respons_skills_0.Replace(",",";");
                                    dataHandler = new SkillsDataHandler();
                                    var result = dataHandler.GetData(respons_skills_0);
                                    csvWriter.WriteField(result);
                                }

                                // Get Availability for this UserId
                                var message = JsonConvert.SerializeObject(new { companyUserId = companyUserId, startDate = today, endDate = untilDate });
                                var httpContent = new StringContent(message, Encoding.UTF8, "application/json");

                                var response_avail = await httpClient_data.PostAsync("https://api.cinode.com/v0.1/companies/140/availability", httpContent);

                                if (response_avail.IsSuccessStatusCode)
                                {
                                    var respons_avail_0 = await response_avail.Content.ReadAsStringAsync();
                                    dataHandler = new AvailabilityDataHandler();
                                    var result = dataHandler.GetData(respons_avail_0);
                                    csvWriter.WriteField(result);
                                }

                                csvWriter.WriteField("https://app.cinode.com/Forefront/organisation/employees/" + user.CompanyUserId + "/" + user.SeoId);
                                csvWriter.WriteField(user.FirstName);
                                csvWriter.WriteField(user.LastName);
                                csvWriter.WriteField(user.FirstName + " " + user.LastName);

                                csvWriter.NextRecord();
                                isConsultant = false;

                            }
                        }

                        // SAVE DATA FROM CSV-FILE TO A .TXT-FILE --------------
                        writer.Flush();
                        var resultOfCsv = Encoding.UTF8.GetString(mem.ToArray());
                        var s = Regex.Replace(resultOfCsv, "\"", string.Empty);
                        System.IO.File.WriteAllText(placeToSaveResult, s);
                        //------------------------------------------------------
                    }
                }
            }
        }

    }
}