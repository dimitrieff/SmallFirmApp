using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmallFirmApp.Models.ProgramModels
{
    public class ProcessedService
    {
        public int Id { get; set; }

        public int VisiteID { get; set; }
        public int ClientId { get; set; } //foreign to Client
        [ValidateNever]
        public Client Client { get; set; } //navigation to client
        public DateTime? Day { get; set; } = DateTime.Today;  //date of perform
        
        public int? ExtraServiceID { get; set; } //foreign to Extraservices
        [ValidateNever]
        public ExtraService? ExtraService { get; set; } //navigation to Extraservices
        
        public int? ExtraCount { get; set; }//number of extras

        public int IsVisited { get; set; } = 0;

        //public double ExtraPrice {  get; set; } //price of extras
    }
}
