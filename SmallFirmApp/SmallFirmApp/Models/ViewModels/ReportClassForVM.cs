namespace SmallFirmApp.Models.ViewModels
{
    public class ReportClassForVM
    {
        public int vId { get; set; }
        public string? Name { get; set; }

        public DateTime? Day { get; set; }

        public string? Staff { get; set; }

        public string? AddOn { get; set; }

        //public string? Consumative { get; set; }

        //public int? ConsCount { get; set; }

        //public double? ConsPrice { get; set; }

        //public string? ExtraService { get; set; }

        //public int ?ExtraCount { get; set; }
        //public double? ExtraPrice { get; set; }

        //public int? visiteId { get; set; }

        public string? ConNameList { get; set; }
        public string? ConCountList { get; set; }
        public string? ConPriceList { get; set; }
        public string? ConOveralList { get; set; }
        public double? ConFinal { get; set; }
        public string? ExtraNameList { get; set; }
        public string? ExtraCountList { get; set; }
        public string? ExtraPriceList { get; set; }
        public string? ExtraOveralList { get; set; }
        public double? ExtraFinal { get; set; }
        public double? Tax { get; set; }
        public double? MonthTax { get; set; }
        public string? Remark { get; set; } = string.Empty;


    }
}
