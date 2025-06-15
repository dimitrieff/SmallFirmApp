using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class ForDeliveryVM
    {
        public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1);
        public DateTime EndDate { get; set; } = DateTime.Now;
        //public bool flag { get; set; } = 
        public IEnumerable<ForDelivery> ForDeliveryList { get; set; }
    }
}
