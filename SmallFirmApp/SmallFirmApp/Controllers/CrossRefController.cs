using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmallFirmApp.Constants;
using SmallFirmApp.Data;
using SmallFirmApp.Models.ProgramModels;
using SmallFirmApp.Models.ViewModels;
using SmallFirmApp.Repositories;

namespace SmallFirmApp.Controllers
{
    public class CrossRefController : Controller
    {
        private static int currentClientId;
        private static DateTime currentDay;
        private static DateTime startDay;
        private static DateTime endDay;
        private static int chosenOne;
        private static int visiteID;
        private static DateTime editDay;
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;

        private SmallFirmAppRepository<Visite> visite { get; set; }
        private SmallFirmAppRepository<Deliveries> deliveries { get; set; }
        private SmallFirmAppRepository<Client> client { get; set; }
        private SmallFirmAppRepository<Staff> staff { get; set; }
        private SmallFirmAppRepository<AddOn> addOn { get; set; }
        private SmallFirmAppRepository<Consumative> consumative { get; set; }
        private SmallFirmAppRepository<ExtraService> extraService { get; set; }
        private SmallFirmAppRepository<ProcessedService> processedService { get; set; }
        private SmallFirmAppRepository<ForDelivery> forDelivery { get; set; }
        private SmallFirmAppRepository<InvoiceDDS> invoiceDDS { get; set; }
        private SmallFirmAppRepository<InvoiceSimple> invoiceSimple { get; set; }
        public CrossRefController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
        {
            visite = new SmallFirmAppRepository<Visite>(ctx);
            client = new SmallFirmAppRepository<Client>(ctx);
            staff = new SmallFirmAppRepository<Staff>(ctx);
            addOn = new SmallFirmAppRepository<AddOn>(ctx);
            consumative = new SmallFirmAppRepository<Consumative>(ctx);
            deliveries = new SmallFirmAppRepository<Deliveries>(ctx);
            extraService = new SmallFirmAppRepository<ExtraService>(ctx);
            processedService = new SmallFirmAppRepository<ProcessedService>(ctx);
            forDelivery = new SmallFirmAppRepository<ForDelivery>(ctx);
            invoiceDDS = new SmallFirmAppRepository<InvoiceDDS>(ctx);
            invoiceSimple = new SmallFirmAppRepository<InvoiceSimple>(ctx);

            _ctx = ctx;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult CrossRefStart()
        {
            return View();
        }
        [Authorize]
        public IActionResult DateInvoice()
        {
            ViewBag.Client = client.GetAllActiveAsync().Result.OrderBy(t => t.Id);
            return View(new DateInvoiceVM());
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DateInvoice(DateInvoiceVM inVM)
        {
            ViewBag.Client = (await client.GetAllActiveAsync()).OrderBy(t => t.Id);
            var cl = await client.GetByIdAsync(inVM.ClientId);
            if (cl.Dds)
            {
                var dds = _ctx.InvoiceDDS.Include(c=> c.Client).Where(v => v.ClientId == cl.Id).
                Where(v => v.DayOfInvoice >= inVM.StartDate && inVM.EndDate >= v.DayOfInvoice).OrderBy(v => v.DayOfInvoice);

                inVM.InvDDS = dds;
                inVM.DDS = true;
            }
            else
            {
                var simple = _ctx.InvoiceSimple.Include(c => c.Client).Where(v => v.ClientId == cl.Id).
                Where(v => v.DayOfInvoice >= inVM.StartDate && inVM.EndDate >= v.DayOfInvoice).OrderBy(v => v.DayOfInvoice);

                inVM.InvSimple = simple;
                inVM.DDS = false;
            }
            
            return View(inVM);
        }
        [Authorize]
        public IActionResult AllInvoice()
        {
            return View(new DateInvoiceVM());
        }

        [Authorize]
        [HttpPost]
        public IActionResult AllInvoice(DateInvoiceVM inVM)
        {
            var dds = _ctx.InvoiceDDS.Include(c => c.Client).
                Where(v => v.DayOfInvoice >= inVM.StartDate && inVM.EndDate >= v.DayOfInvoice).OrderBy(v => v.DayOfInvoice);
            var simple = _ctx.InvoiceSimple.Include(c => c.Client).
                Where(v => v.DayOfInvoice >= inVM.StartDate && inVM.EndDate >= v.DayOfInvoice).OrderBy(v => v.DayOfInvoice);
            inVM.InvDDS = dds;
            inVM.InvSimple = simple;
            inVM.ClientId = 1;
            return View(inVM);
        }

        [Authorize]
        public IActionResult VisiteForTime()
        {
            return View(new VisiteVM());
        }

        [Authorize]
        [HttpPost]
        public IActionResult VisiteForTime(VisiteVM vVM)
        {
            vVM.VisiteList = _ctx.Visite.Include(c => c.Client).
                Where(v => v.Day >= vVM.StartDate && vVM.EndDate >= v.Day).OrderBy(v => v.Day);
            vVM.Flag = 1;
            return View(vVM);
        }

        [Authorize]
        public IActionResult DateDeliveries()
        {
            return View(new DeliveryVM());
        }

        [Authorize]
        [HttpPost]
        public IActionResult DateDeliveries(DeliveryVM dVM )
        {
            dVM.DateDeliver = _ctx.Deliveries.Include(d => d.Client).Include(d => d.Consumative).Where(d => d.ConsCount > 0).
                Where(d => d.Day >= dVM.StartDate && dVM.EndDate >= d.Day).OrderBy(d => d.Day);
            dVM.flag = true;
            return View(dVM);
        }

        [Authorize]
        public IActionResult ForDeliverList()
        {
            var fd = new ForDeliveryVM();
            fd.ForDeliveryList = _ctx.ForDelivery.Include(d => d.Client).
                Where(d => d.Day >= fd.StartDate && fd.EndDate >= d.Day).OrderBy(d => d.Day);

            return View(fd);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ForDeliverList(ForDeliveryVM fd)
        {
            fd.ForDeliveryList = _ctx.ForDelivery.Include(d => d.Client).
            Where(d => d.Day >= fd.StartDate && fd.EndDate >= d.Day).OrderBy(d => d.Day);
           // fd.flag = true;
            return View(fd);
        }

        [Authorize]
        public async Task<IActionResult> EditForDev(int id)
        {
            ForDelivery f = await forDelivery.GetByIdAsync(id);
            f.Client = await client.GetByIdAsync(f.ClientId);
            
            ViewBag.Consumatives = consumative.GetAllActiveAsync().Result.OrderBy(c => c.Id);
            return View(f);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditForDev(ForDelivery f)
        {

            await forDelivery.UpdateAsync(f);
            return RedirectToAction("ForDeliverList");
        }

        [Authorize]
        public async Task<IActionResult> DeleteForDev(int id)
        {
           
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole(Roles.Admin) == false)
            {
                return Forbid();
            }
            await forDelivery.DeleteAsync(id);

            return Ok();
        }

        [Authorize]
        public IActionResult StaffVisite()
        {
            ViewBag.Staff = staff.GetAllActiveAsync().Result.OrderBy(t => t.Id);
            return View(new StaffVM());
        }

        [Authorize]
        [HttpPost]
        public IActionResult StaffVisite(StaffVM sVM)
        {
            ViewBag.Staff = staff.GetAllActiveAsync().Result.OrderBy(t => t.Id);
            sVM.VisiteList = _ctx.Visite.Include(s => s.Client).
                Where(s => s.Day >= sVM.StartDate && sVM.EndDate >= s.Day).
                Where(s => s.Staff.Contains(sVM.StaffName)).OrderBy(s => s.Day);
            
            sVM.Flag = true;
            return View(sVM);
        }
    }
}
