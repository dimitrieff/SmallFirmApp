using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class StaffVM
    {
        public string StaffName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public IEnumerable<Visite> VisiteList { get; set; }
        public bool Flag { get; set; }
    }
}
