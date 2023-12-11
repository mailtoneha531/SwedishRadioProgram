using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SwedishRadioProgram.Models;

namespace SwedishRadioProgram.Services
{
    public class SwedishRadioService
    {
        private readonly HttpClient _httpClient;

        public SwedishRadioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.sr.se/");
        }


        public async Task<int> GetHumorCategoryId()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/v2/programcategories?format=json&pagination=false");
            if (response.IsSuccessStatusCode)
            {

                ProgramCategoryModel categories = await response.Content.ReadFromJsonAsync<ProgramCategoryModel>();
                foreach (var category in categories.ProgramCategories)
                {
                    if (category.Name.Equals("Humor", StringComparison.OrdinalIgnoreCase))
                    {
                        return category.Id;
                    }
                }
            }
            return -1;
        }

        public DateTime getFormattedDated(string jsonDate)
        {
            long ticks = long.Parse(jsonDate.Substring(6, jsonDate.Length - 8));

            // Convert ticks to milliseconds and create a DateTimeOffset
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(ticks);

            // Convert to DateTime if needed (considering the UTC date)
            return dateTimeOffset.UtcDateTime;
        }

      

        public async Task<List<ProgramDetails>> GetHumorProgramsWithPodcasts()
        {
            // Fetch programs from the API
            HttpResponseMessage response = await _httpClient.GetAsync("api/v2/programs/index?format=json&pagination=false");
            int humorCategoryId = await GetHumorCategoryId();
            if (response.IsSuccessStatusCode)
            {
                var programs = await response.Content.ReadFromJsonAsync<ProgramApiResponse>();
                if (programs != null && programs.Programs != null)
                {
                    // Filter all the programs belonging to the "Humor" category
                    var humorPrograms = programs.Programs.Where(p =>
                        p.ProgramCategory!=null ? p.ProgramCategory.Id == humorCategoryId:false && 
                        p.Haspod &&
                        !p.Archived);

                    // Group programs by channel
                    var groupedPrograms = humorPrograms.GroupBy(p => p.Channel.Name);

                    // Retrieve the last three episodes for each program
                    List<ProgramDetails> ProgramDetailss = new List<ProgramDetails>();
                    foreach (var group in groupedPrograms)
                    {
                        foreach (var program in group)
                        {
                            // Fetch last three podcast episodes for each program
                            var episodesResponse = await _httpClient.GetAsync($"api/v2/podfiles?programid={program.Id}&format=json");
                            if (episodesResponse.IsSuccessStatusCode)
                            {
                                var podFiles = await episodesResponse.Content.ReadFromJsonAsync<EpisodesApiResponse>();
                                var lastThreeEpisodes = podFiles?.PodFiles?.Take(3).ToList();

                                if (lastThreeEpisodes != null && lastThreeEpisodes.Any())
                                {
                                    var ProgramDetails = new ProgramDetails
                                    {
                                        Name = program.Name,
                                        Image = program.programimage,
                                        Description = program.Description,
                                        LastThreeEpisodes = lastThreeEpisodes.Select(e => new PodcastEpisode
                                        {
                                            Title = e.Title,
                                            PublicationDate = getFormattedDated(e.Publishdateutc),
                                            Duration = TimeSpan.FromSeconds(e.Duration),
                                            AudioUrl = e.Url
                                        }).ToList()
                                    };
                                    ProgramDetailss.Add(ProgramDetails);
                                }
                            }
                        }
                    }
                    return ProgramDetailss;
                }
            }
            return null;
        }
    }

}
