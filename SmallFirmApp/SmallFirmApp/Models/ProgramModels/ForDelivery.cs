using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmallFirmApp.Models.ProgramModels
{
    public class ForDelivery
    {
        public int Id { get; set; }
        public int ClientId { get; set; } //foreign to Client
        
        [ValidateNever]
        public Client Client { get; set; } //navigation
        public DateTime Day { get; set; } = DateTime.Today;   //date of delivery
        public string ForDeliver { get; set; }// za dostawka

    }
}
