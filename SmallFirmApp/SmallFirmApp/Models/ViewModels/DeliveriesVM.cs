using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class DeliveriesVM
    {
        public int EditId { get; set; }
        public DateTime EditDay { get; set; }
        public int EditClientId { get; set; }

        public int isVisited { get; set; }
        public int isVisitedId { get; set; }
        public int[] EditCons { get; set; }

        public int[] ConsID { get; set; }

        public string[] ConsName { get; set; }

        public int[] consDelivered { get; set; }
        public IEnumerable<Consumative>? EditList { get; set; }

        public List<ForDelivery>? PreviousAbsent { get; set; }
    }
}


