using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;
namespace SmallFirmApp.Models.ProgramModels
{
    public class ExtraService
    {
        public int Id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public int isActive { get; set; } = 1;
        public string? Name { get; set; }
        [Required(ErrorMessage = "Цифри и десетична ТОЧКА")]
        public double? ExtraPrice { get; set; }
    }
}
