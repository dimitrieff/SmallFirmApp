using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class VisiteVM
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public IEnumerable<Visite> VisiteList { get; set; }
        public int Flag {  get; set; }
    }
}
