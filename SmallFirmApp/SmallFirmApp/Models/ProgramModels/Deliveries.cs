using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SmallFirmApp.Models.ProgramModels
{
    public class Deliveries
    {
        public int Id { get; set; }
        public int VisiteID { get; set; }
        public int ClientId { get; set; } //foreign to Client
        [ValidateNever]
        public Client Client { get; set; } //navigation to Client
        public DateTime? Day { get; set; } = DateTime.Today;  //date of delivery
       
        public int? ConsumativeId { get; set; } //foreign to Consumative
        [ValidateNever]
        public Consumative? Consumative { get; set; }  //navigation to Consumatives
        public int? ConsCount { get; set; } //consumatives delivered count
        [Required]
        public string UserId { get; set; } //forein to Identity
        public IdentityUser User { get; set; } //navigation to Idenity

        public int isVisited { get; set; } = 0;

    }
}
