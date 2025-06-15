using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmallFirmApp.Models.ProgramModels
{
    public class InvoiceDDS
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int ClientId { get; set; } //foreign to Client
        [ValidateNever]
        public Client Client { get; set; } //navigation to Client
        public DateTime DayOfInvoice { get; set; }
        public double Price { get; set; }
    }
}
