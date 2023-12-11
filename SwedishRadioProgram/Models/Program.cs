using System.Threading.Channels;

namespace SwedishRadioProgram.Models
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string programimage { get; set; }
        public string Description { get; set; }
        public ProgramCategory ProgramCategory { get; set; }
        public bool Archived { get; set; }

        public bool Haspod { get; set; }
        
        public Channel Channel { get; set; }
        //public List<Podfile> Podfiles { get; set; }
    }
}
