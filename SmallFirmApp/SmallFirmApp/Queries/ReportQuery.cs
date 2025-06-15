using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using SmallFirmApp.Data;
using SmallFirmApp.Models.ViewModels;

namespace SmallFirmApp.Queries
{
    public class ReportQuery
    {
        private ApplicationDbContext _ct;
        public  ReportQuery(ApplicationDbContext ct)
        {
                _ct = ct;

        }
       

        public  ReportVM GetQuery(int chosenOne, DateTime start, DateTime end)//query for join tables
        {
            //var query = (
            //       from v in _ct.Visite
            //       join d in _ct.Deliveries
            //       on new { v.ClientId, v.Day } equals new { d.ClientId, d.Day } into temp

            //       from t in temp.DefaultIfEmpty()
            //       join c in _ct.Consumative
            //       on new { t.ConsumativeId } equals new { ConsumativeId = c.Id }

            //       where (v.ClientId == chosenOne && v.Day >= start && end >= v.Day)
            //       orderby v.Day

            //       select new ReportClassForVM
            //       {
            //           Name = v.Client.Name != null ? t.Client.Name : string.Empty,
            //           Day = v.Day,
            //           Staff = v.Staff,
            //           AddOn = v.AddOn != null ? v.AddOn : string.Empty,
            //           Consumative = t.Consumative.Name != null ? t.Consumative.Name : string.Empty,
            //           ConsCount = t.ConsCount != null ? t.ConsCount : 0,
            //           ConsPrice = c.Price
            //       }).AsEnumerable();

            var model = new ReportVM();// { ReportDataList = query };

            return (model);

        }
    }
}
