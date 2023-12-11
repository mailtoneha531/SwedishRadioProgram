namespace SwedishRadioProgram.Models
{
    public class PodcastEpisode
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string AudioUrl { get; set; }
    }
}
