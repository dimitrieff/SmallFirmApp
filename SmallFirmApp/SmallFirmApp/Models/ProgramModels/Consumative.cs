using DocumentFormat.OpenXml.Wordprocessing;
//using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Data.SqlTypes;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace SmallFirmApp.Models.ProgramModels
{
    public class Consumative
    {
        public int Id { get; set; }
        public int isActive { get; set; } = 1;
        public DateTime date { get; set; }=DateTime.Now;
        public string? Name { get; set; }
        [Required(ErrorMessage = "Цифри и десетична ТОЧКА")]
        
        public double? Price { get; set; }
    }
}
