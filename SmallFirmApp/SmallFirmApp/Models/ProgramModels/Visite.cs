using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmallFirmApp.Models.ProgramModels
{
    public class Visite
    {
        public int Id { get; set; }
        [Required]
        [Range(2, 250, ErrorMessage = "Изберете клиент")]
        public int ClientId { get; set; }//foreign to Client
        [ValidateNever]
        public Client Client { get; set; } //navigation to Client
        public DateTime? Day { get; set; } = DateTime.Today;   //date of visite
        [Required(ErrorMessage = "Изберете служител")]
        public string Staff { get; set; } //staff on a visite
        [ValidateNever]
        public string AddOn { get; set; } //AddOns executed
        [ValidateNever]
        public string UserId { get; set; } //user logged in
        [ValidateNever]
        [ForeignKey(nameof(UserId))] //user name
        public IdentityUser User { get; set; }// navigation to Identity
        [ValidateNever]
        public string Remark { get; set; }//notes for visite
        public bool OnMonth {  get; set; }//peyment method
        public double Tax { get; set; } //price 
    }
}
