using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class ReportVM
    {
        public int? ChosenId { get; set; } = 0;
        public DateTime? Checker { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime? EndDate { get; set; } = DateTime.Now.ToLocalTime();
        
        public IEnumerable<ReportClassForVM>? ReportDataList { get; set; }

        public IEnumerable<Visite>? ReportVisite { get; set; }

        public IEnumerable<Deliveries>? ReportDelivery { get; set; }

        public IEnumerable<ProcessedService>? ReportProcess { get; set; }

        

        //public IEnumerable<int> ReportPrices { get; set; }

        //public int? vCounter { get; set; }

        //public int? dCounter { get; set; }
        //public int? pCounter { get; set; }


    }
}