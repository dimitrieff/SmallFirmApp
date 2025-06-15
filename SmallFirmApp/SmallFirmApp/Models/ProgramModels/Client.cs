using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmallFirmApp.Models.ProgramModels
{
    public class Client
    {
        public int Id { get; set; }
        public int isActive { get; set; } = 1;
        public string Name { get; set; } = string.Empty;
        [ValidateNever]
        public string? City { get; set; } = string.Empty;
        [ValidateNever]
        public string? Address { get; set; } = string.Empty;
        public bool Dds { get; set; }
        [ValidateNever]
        public string? Ein { get; set; } = string.Empty;
        [ValidateNever]
        public string? Recipient {  get; set; } = string.Empty;
        [ValidateNever]
        public string? Mol { get; set; } = string.Empty;
        [ValidateNever]
        public string? Email { get; set; } = string.Empty;
        public bool OnMonth { get; set; }
        public double Tax { get; set; }

    }
}
