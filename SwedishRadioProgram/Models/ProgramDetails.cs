namespace SwedishRadioProgram.Models
{
    public class ProgramDetails
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public List<PodcastEpisode> LastThreeEpisodes { get; set; }
    }
}
