using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class DateInvoiceVM
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int ClientId { get; set; }
        public bool DDS { get; set; }
        public IEnumerable<InvoiceDDS>? InvDDS { get; set; }
        public IEnumerable<InvoiceSimple>? InvSimple { get; set; }
    }
}
