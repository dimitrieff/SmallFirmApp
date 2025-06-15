using SmallFirmApp.Models.ProgramModels;

namespace SmallFirmApp.Models.ViewModels
{
    public class InvoiceVM
    {
        public Client Client { get; set; }
        public DateTime InvoiceDate { get; set; }

        public double Price { get; set; }

        public string PriceByWords { get; set; }

        public int InvoiceNumber { get; set; }

        public string InvoiceType {  get; set; }

    }
}
