using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class DeliveryVM
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public bool flag {  get; set; }
        public IEnumerable<Deliveries> DateDeliver {  get; set; }
    }
}
