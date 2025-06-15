namespace SmallFirmApp.Models.ProgramModels
{
    public class Staff
    {
        public int Id { get; set; }
        public int isActive { get; set; } = 1;
        public string Name { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
    }
}
