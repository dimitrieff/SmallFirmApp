using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using SmallFirmApp.Constants;
using SmallFirmApp.Data;
using SmallFirmApp.Models.ProgramModels;
using SmallFirmApp.Models.ViewModels;
using SmallFirmApp.Queries;
using SmallFirmApp.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using SmallFirmApp.AdditionalFunctions;

namespace SmallFirmApp.Controllers
{
    public class SmallFirmAppController : Controller
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
        public SmallFirmAppController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
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
        public IActionResult Index()//New enty
        {
            this.LoadViewBag();
            return View("Index", new Visite());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(Visite v)//New entry processing
        {
            currentClientId = v.ClientId;

            currentDay = v.Day.Value;
            v.UserId = _userManager.GetUserId(User);
            v.Tax = (await client.GetByIdAsync(v.ClientId)).Tax;
            v.OnMonth = (await client.GetByIdAsync(v.ClientId)).OnMonth;
            if (ModelState.IsValid)
            {
                
                if (v.AddOn == null)
                {
                    v.AddOn = string.Empty;
                }
                if (v.Remark == null)
                {
                    v.Remark = string.Empty;
                }

                await visite.AddAsync(v);
                visiteID = v.Id;

                return RedirectToAction("Deliveries", "SmallFirmApp");
            }
            else
            {
                this.LoadViewBag();
                return View("Index", v);
            }
        }
        [Authorize]
        public IActionResult Deliveries()//new deliveries
        {
            DeliveriesVM deliveriesVM = new DeliveriesVM();

            //var consForVM = _ctx.Consumative.Where(c => c.isActive !=0).AsEnumerable();
            var consForVM = consumative.GetAllActiveAsync().Result;
            int cons = consForVM.Count();

            deliveriesVM.ConsName = new string[cons];
            deliveriesVM.ConsID = new int[cons];
            deliveriesVM.consDelivered = new int[cons];
            int id = 0;

            foreach (var item in consForVM)
            {
                deliveriesVM.ConsID[id] = item.Id;
                deliveriesVM.ConsName[id] = item.Name;
                id++;
            }
                                    
            var absent = _ctx.ForDelivery.Include(i =>i.Client).
                Where(d => d.ClientId == currentClientId).OrderBy(d => d.Day).
                Select(i => new ForDelivery { Id = i.Id,
                    ClientId = i.ClientId, 
                    Client = i.Client, 
                    Day = i.Day, 
                ForDeliver = i.ForDeliver}).ToList();

            deliveriesVM.PreviousAbsent = absent;

            return View(deliveriesVM);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Deliveries(DeliveriesVM dVM)//new deliveries processing
        {
            int tempcount = dVM.consDelivered.Length;
            int check = 0;
            for (int i = 0; i < tempcount; i++)
            {
                if (dVM.consDelivered[i] != 0)
                {
                    Deliveries d = new Deliveries();
                    d.ClientId = currentClientId;
                    d.Day = currentDay;
                    d.ConsumativeId = dVM.ConsID[i];
                    d.ConsCount = dVM.consDelivered[i];
                    d.UserId = _userManager.GetUserId(User);
                    d.VisiteID = visiteID;
                    await deliveries.AddAsync(d);
                    check++;
                }
            }
            if (check == 0)
            {
                Deliveries d = new Deliveries();
                d.ClientId = currentClientId;
                d.Day = currentDay;
                d.ConsumativeId = 1;
                d.ConsCount = 0;
                d.UserId = _userManager.GetUserId(User);
                d.isVisited = 1;
                d.VisiteID = visiteID;
                await deliveries.AddAsync(d);
            }

            if (dVM.PreviousAbsent != null)
            {
                for (int i = 0; i < dVM.PreviousAbsent.Count(); i++)
                {
                    if (dVM.PreviousAbsent[i].ForDeliver != "" && dVM.PreviousAbsent[i].ForDeliver != null)
                    {
                        ForDelivery absent = new ForDelivery();
                        absent.Id = dVM.PreviousAbsent[i].Id;
                        absent.ClientId = dVM.PreviousAbsent[i].ClientId;
                        absent.Day = dVM.PreviousAbsent[i].Day;
                        absent.ForDeliver = dVM.PreviousAbsent[i].ForDeliver;
                        await forDelivery.UpdateAsync(absent);
                    }
                    else
                    {
                        await forDelivery.DeleteAsync(dVM.PreviousAbsent[i].Id);
                    }
                }
            }

            return RedirectToAction("ProcessedServices");
        }
        [Authorize]
        public IActionResult ProcessedServices()//new processed services
        {
            PServicesVM psVM = new PServicesVM() { };

            var servicesForVM = extraService.GetAllActiveAsync().Result.OrderBy(c => c.Id);

            int service = servicesForVM.Count();

            psVM.ServiceName = new string[service];
            psVM.ServiceID = new int[service];
            psVM.ServiceProcessed = new int[service];
            int id = 0;

            foreach (var item in servicesForVM)
            {
                psVM.ServiceID[id] = item.Id;
                psVM.ServiceName[id] = item.Name;
                id++;
            }
            return View(psVM);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcessedServices(PServicesVM psVM)//new services processing
        {
            int tempcount = psVM.ServiceProcessed.Length;
            int check = 0;

            for (int i = 0; i < tempcount; i++)
            {
                if (psVM.ServiceProcessed[i] != 0)
                {
                    ProcessedService e = new ProcessedService();
                    e.ClientId = currentClientId;
                    e.Day = currentDay;
                    e.ExtraServiceID = psVM.ServiceID[i];
                    e.ExtraCount = psVM.ServiceProcessed[i];
                    e.VisiteID = visiteID;
                    await processedService.AddAsync(e);
                    check++;
                }
            }
            if (check == 0)
            {
                ProcessedService e = new ProcessedService();
                e.ClientId = currentClientId;
                e.Day = currentDay;
                e.ExtraServiceID = _ctx.ExtraService.OrderBy(c => c.Id).FirstOrDefault().Id;
                e.ExtraCount = 0;
                e.IsVisited = 1;
                e.VisiteID = visiteID;
                await processedService.AddAsync(e);

            }
            return RedirectToAction("ForDeliver");
        }

        [Authorize]
        public IActionResult ForDeliver()//new for delivery
        {
            ViewBag.Consumatives = consumative.GetAllActiveAsync().Result.OrderBy(c => c.Id);
            return View(new ForDelivery());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ForDeliver(ForDelivery f)//processing new for delivery
        {
            if (f.ForDeliver != null)
            {
                f.ClientId = currentClientId;
                f.Day = currentDay;
                await forDelivery.AddAsync(f);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Report()//report waiting for selection
        {
            ViewBag.Client = client.GetAllActiveAsync().Result.OrderBy(c => c.Id);
            return View(new ReportVM());
        }

        [Authorize ]
        [HttpPost]
        public IActionResult Report(ReportVM vm)//processing selected report
        {
            chosenOne = vm.ChosenId.Value;
            startDay = vm.StartDate.Value;
            endDay = vm.EndDate.Value;
            ViewBag.Client = client.GetAllActiveAsync().Result.OrderBy(c => c.Id);
            var model = GetQuery(chosenOne, startDay, endDay);
            model.ChosenId = chosenOne;
            return View(model);
        }

        [Authorize(Roles = "Admin, StaffOperator")]
        public IActionResult InvoiceOnDemand()
        {
            ViewBag.Client = client.GetAllActiveAsync().Result.OrderBy(c => c.Id);
            return View(new InvoiceOnDemandVM());
        }

        [Authorize(Roles = "Admin, StaffOperator")]
        [HttpPost]
        public IActionResult InvoiceOnDemand(InvoiceOnDemandVM vm)
        {
            chosenOne = vm.ChosenId.Value;
            string price11 = vm.Price;
            //Invoice(price11);
            return RedirectToAction("Invoice", new { price = price11 });
        }

        [Authorize(Roles = "Admin, Operator")]

        public IActionResult Invoice(string price)
        {
            InvoiceVM inVM = new InvoiceVM();
            inVM.Client = client.GetByIdAsync(chosenOne).Result;
            double numberPrice = Convert.ToDouble(price);
            inVM.InvoiceDate = DateTime.Now;
            inVM.Price = numberPrice;
            inVM.PriceByWords = Convertor.ConvertToLetter(numberPrice);

            if (inVM.Client.Dds)
            {
                inVM.InvoiceType = "Фактура по ДДС";
                var lastNumber = invoiceDDS.GetAllAsync().Result;
                inVM.InvoiceNumber = lastNumber.OrderByDescending(i => i.Id).FirstOrDefault().InvoiceNumber + 1;
            }
            else
            {
                inVM.InvoiceType = "Опростена фактура";
                var lastNumber = invoiceSimple.GetAllAsync().Result;
                inVM.InvoiceNumber = lastNumber.OrderByDescending(i => i.Id).FirstOrDefault().InvoiceNumber + 1;
            }

            return View(inVM);
        }

        [Authorize (Roles = "Admin, StaffOperator")]
        [HttpPost]
        public async Task <IActionResult> Invoice(InvoiceVM inVM)
        {
            inVM.Client = await client.GetByIdAsync(chosenOne);
            if (inVM.Client.Dds)
            {
                InvoiceDDS inDDS = new InvoiceDDS {
                    InvoiceNumber = inVM.InvoiceNumber,
                    ClientId = inVM.Client.Id,
                    DayOfInvoice = inVM.InvoiceDate,
                    Price = inVM.Price };
                await invoiceDDS.AddAsync(inDDS);
                
                return RedirectToAction("InvoiceDDSexport", inVM);
            }
            else
            {
                InvoiceSimple inSimple = new InvoiceSimple
                {
                    InvoiceNumber = inVM.InvoiceNumber,
                    ClientId = inVM.Client.Id,
                    DayOfInvoice = inVM.InvoiceDate,
                    Price = inVM.Price
                };
                await invoiceSimple.AddAsync(inSimple);

                return RedirectToAction("InvoiceSimpleExport", inVM);
            }
        }

        
        [Authorize(Roles = "Admin, StaffOperator")]
        public IActionResult InvoiceDDSexport(InvoiceVM inVM)
        {
            inVM.Client = client.GetByIdAsync(chosenOne).Result;
            return View(inVM);
        }
        [Authorize(Roles = "Admin, StaffOperator")]
        public IActionResult InvoiceSimpleExport(InvoiceVM inVM)
        {
            inVM.Client = client.GetByIdAsync(chosenOne).Result;
            return View(inVM);
        }

        [Authorize]
        public async Task<IActionResult> EditEntry(int id)
        {
            visiteID = id;
            var vc = _ctx.Visite.Include(c => c.Client).Where(v => v.Id == id);

            ViewBag.Staff = await staff.GetAllActiveAsync();
            ViewBag.AddOn = await addOn.GetAllActiveAsync();

            Visite v = new Visite();

            v.Id = vc.ElementAt(0).Id;
            v.ClientId = vc.ElementAt(0).ClientId;
            v.Client = vc.ElementAt(0).Client;
            v.Day = vc.ElementAt(0).Day;
            v.Staff = vc.ElementAt(0).Staff;
            v.AddOn = vc.ElementAt(0).AddOn;
            v.UserId = vc.ElementAt(0).UserId;
            v.User = vc.ElementAt(0).User;
            v.Remark = vc.ElementAt(0).Remark;
            v.Tax = vc.ElementAt(0).Tax;
            return View(v);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditEntry(Visite v)
        {
            if (v.AddOn == null)
            {
                v.AddOn = string.Empty;
            }
            if (v.Remark == null)
            {
                v.Remark = string.Empty;
            }
            await visite.UpdateAsync(v);
            editDay = v.Day.Value;

            return RedirectToAction("EditDeliveries");
        }

        [Authorize]
        public IActionResult EditDeliveries()
        {
            var dc = _ctx.Deliveries.Include(c => c.Consumative).Where(v => v.VisiteID == visiteID).OrderBy(c => c.ConsumativeId);

            DeliveriesVM deliveriesVM = new DeliveriesVM();
            deliveriesVM.EditId = visiteID; //id na pose]enieto
            deliveriesVM.EditDay = editDay;//den na pose]enie
            deliveriesVM.EditClientId = dc.ElementAt(0).ClientId;//klient

            var consForVM = consumative.GetAllActiveAsync().Result.OrderBy(c => c.Id);

            int cons = consForVM.Count();

            deliveriesVM.ConsName = new string[cons];
            deliveriesVM.ConsID = new int[cons];
            deliveriesVM.consDelivered = new int[cons];
            deliveriesVM.isVisited = 0;
            deliveriesVM.EditCons = new int[cons];//masiw s id na napraweni we`e dostawki
            int id = 0;

            foreach (var item in consForVM)
            {
                deliveriesVM.ConsID[id] = item.Id;
                deliveriesVM.ConsName[id] = item.Name;
                id++;
            }
            for (int i = 0; i < dc.Count(); i++)
            {
                if (dc.ElementAt(i).isVisited != 0)
                {
                    deliveriesVM.isVisited = 1;
                    deliveriesVM.isVisitedId = dc.ElementAt(i).Id;
                }
                for (int j = 0; j < cons; j++)
                {
                    try
                    {
                        if (deliveriesVM.ConsID[j] == dc.ElementAt(i).ConsumativeId)
                        {
                            deliveriesVM.consDelivered[j] = dc.ElementAt(i).ConsCount.Value;
                            deliveriesVM.EditCons[j] = dc.ElementAt(i).Id;
                        }
                    }
                    catch
                    {
                        deliveriesVM.consDelivered[j] = 0;
                        deliveriesVM.EditCons[j] = 0;
                        continue;
                    }
                }
            }
            return View(deliveriesVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditDeliveries(DeliveriesVM dVM)//edit consumative processing
        {
            int check = 0;
            int tempcount = dVM.consDelivered.Length;
            foreach (var d in dVM.EditCons)
            {
                if (d != 0)
                {
                    check++;
                }
            }

            for (int i = 0; i < tempcount; i++)
            {
                if (dVM.consDelivered[i] != 0 && (dVM.EditCons[i] == 0))
                {
                    Deliveries d = new Deliveries();
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ConsumativeId = dVM.ConsID[i];
                    d.ConsCount = dVM.consDelivered[i];
                    d.UserId = _userManager.GetUserId(User);
                    d.VisiteID = dVM.EditId;
                    await deliveries.AddAsync(d);
                    if (dVM.isVisited != 0)
                    {
                        await deliveries.DeleteAsync(dVM.isVisitedId);
                        dVM.isVisited = 0;
                    }
                }
                if (dVM.consDelivered[i] != 0 && (dVM.EditCons[i] != 0))
                {
                    Deliveries d = new Deliveries();
                    d.Id = dVM.EditCons[i];
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ConsumativeId = dVM.ConsID[i];
                    d.ConsCount = dVM.consDelivered[i];
                    d.UserId = _userManager.GetUserId(User);
                    d.VisiteID = dVM.EditId;
                    await deliveries.UpdateAsync(d);
                }
                if (dVM.consDelivered[i] == 0 && (dVM.EditCons[i] != 0) && dVM.isVisited == 0)
                {
                    await deliveries.DeleteAsync(dVM.EditCons[i]);
                    check--;
                }
                if (dVM.consDelivered[i] == 0 && (dVM.EditCons[i] != 0) && (dVM.isVisited != 0))
                {
                    Deliveries d = new Deliveries();
                    d.Id = dVM.isVisitedId;
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ConsumativeId = _ctx.Consumative.OrderBy(c => c.Id).FirstOrDefault().Id;
                    d.ConsCount = 0;
                    d.UserId = _userManager.GetUserId(User);
                    d.isVisited = 1;
                    d.VisiteID = dVM.EditId;
                    await deliveries.UpdateAsync(d);
                }
            }
            if (check == 0)
            {
                Deliveries d = new Deliveries();
                d.ClientId = dVM.EditClientId;
                d.Day = editDay;
                d.ConsumativeId = _ctx.Consumative.OrderBy(c => c.Id).FirstOrDefault().Id;
                d.ConsCount = 0;
                d.UserId = _userManager.GetUserId(User);
                d.isVisited = 1;
                d.VisiteID = dVM.EditId;
                await deliveries.AddAsync(d);
            }
            return RedirectToAction("EditServices");
        }
        [Authorize]
        public IActionResult EditServices()
        {
            var pc = _ctx.ProcessedService.Include(c => c.ExtraService).Where(v => v.VisiteID == visiteID).OrderBy(c => c.ExtraServiceID);

            DeliveriesVM deliveriesVM = new DeliveriesVM();
            deliveriesVM.EditId = visiteID; //id na pose]enieto
            deliveriesVM.EditDay = editDay;//den na pose]enie
            deliveriesVM.EditClientId = pc.ElementAt(0).ClientId;//klient

            var consForVM = extraService.GetAllActiveAsync().Result.OrderBy(c => c.Id);

            int cons = consForVM.Count();

            deliveriesVM.ConsName = new string[cons];
            deliveriesVM.ConsID = new int[cons];
            deliveriesVM.consDelivered = new int[cons];
            deliveriesVM.isVisited = 0;
            deliveriesVM.EditCons = new int[cons];//masiw s id na napraweni we`e dostawki
            int id = 0;

            foreach (var item in consForVM)
            {
                deliveriesVM.ConsID[id] = item.Id;
                deliveriesVM.ConsName[id] = item.Name;
                id++;
            }
            for (int i = 0; i < pc.Count(); i++)
            {
                if (pc.ElementAt(i).IsVisited != 0)
                {
                    deliveriesVM.isVisited = 1;
                    deliveriesVM.isVisitedId = pc.ElementAt(i).Id;
                }

                for (int j = 0; j < cons; j++)
                {
                    try
                    {
                        if (deliveriesVM.ConsID[j] == pc.ElementAt(i).ExtraServiceID)
                        {
                            deliveriesVM.consDelivered[j] = pc.ElementAt(i).ExtraCount.Value;
                            deliveriesVM.EditCons[j] = pc.ElementAt(i).Id;
                        }
                    }
                    catch
                    {
                        deliveriesVM.consDelivered[j] = 0;
                        deliveriesVM.EditCons[j] = 0;
                        continue;
                    }
                }
            }
            return View(deliveriesVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditServices(DeliveriesVM dVM)//edit services processing
        {
            int check = 0;
            int tempcount = dVM.consDelivered.Length;
            foreach (var d in dVM.EditCons)
            {
                if (d != 0)
                {
                    check++;
                }
            }

            for (int i = 0; i < tempcount; i++)
            {
                if (dVM.consDelivered[i] != 0 && (dVM.EditCons[i] == 0))//wywevdane na konsumatiw kojto e wyweden no promenen
                {
                    ProcessedService d = new ProcessedService();
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ExtraServiceID = dVM.ConsID[i];
                    d.ExtraCount = dVM.consDelivered[i];
                    //d. = _userManager.GetUserId(User);
                    d.VisiteID = dVM.EditId;
                    await processedService.AddAsync(d);
                    if (dVM.isVisited != 0)
                    {
                        await processedService.DeleteAsync(dVM.isVisitedId);
                        dVM.isVisited = 0;
                    }
                }
                if (dVM.consDelivered[i] != 0 && (dVM.EditCons[i] != 0))
                {
                    ProcessedService d = new ProcessedService();
                    d.Id = dVM.EditCons[i];
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ExtraServiceID = dVM.ConsID[i];
                    d.ExtraCount = dVM.consDelivered[i];
                    //d.UserId = _userManager.GetUserId(User);
                    d.VisiteID = dVM.EditId;
                    await processedService.UpdateAsync(d);

                }
                if (dVM.consDelivered[i] == 0 && (dVM.EditCons[i] != 0) && dVM.isVisited == 0)
                {
                    await processedService.DeleteAsync(dVM.EditCons[i]);
                    check--;
                }
                if (dVM.consDelivered[i] == 0 && (dVM.EditCons[i] != 0) && (dVM.isVisited != 0))
                {
                    ProcessedService d = new ProcessedService();
                    d.Id = dVM.isVisitedId;
                    d.ClientId = dVM.EditClientId;
                    d.Day = editDay;
                    d.ExtraServiceID = _ctx.ExtraService.OrderBy(c => c.Id).FirstOrDefault().Id;
                    d.ExtraCount = 0;
                    //d.UserId = _userManager.GetUserId(User);
                    d.IsVisited = 1;
                    d.VisiteID = dVM.EditId;
                    await processedService.UpdateAsync(d);
                }
            }
            if (check == 0)
            {
                ProcessedService d = new ProcessedService();
                d.ClientId = dVM.EditClientId;
                d.Day = editDay;
                d.ExtraServiceID = _ctx.ExtraService.OrderBy(c => c.Id).FirstOrDefault().Id;
                d.ExtraCount = 0;
                //d.UserId = _userManager.GetUserId(User);
                d.IsVisited = 1;
                d.VisiteID = dVM.EditId;
                await processedService.AddAsync(d);
            }

            return RedirectToAction("Report");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var vc = await visite.GetByIdAsync(id);
            var pc = _ctx.ProcessedService.Where(v => v.VisiteID == id);
            var dc = _ctx.Deliveries.Where(v => v.VisiteID == id);

            if (vc == null || pc == null || dc == null)
            {
                return NotFound();
            }
            //var userId = _userManager.GetUserId(User);

            //if (User.IsInRole(Roles.Admin) == false)
            //{
            //    return Forbid();
            //}
            await visite.DeleteAsync(id);

            foreach (var d in dc)
            {
                await deliveries.DeleteAsync(d.Id);
            }
            foreach (var p in pc)
            {
                await processedService.DeleteAsync(p.Id);
            }
            return Ok();
        }
        
        //---------------------------------EXCEL REpooooort--------------------------------
        [Authorize(Roles = "Admin, StaffOperator")]
        [HttpGet]
        public FileResult ExcelExport(string Discount, double DiscountValue)//creating excel file
        {
            string discount = Discount != null ? Discount : string.Empty;
            double discountValue = DiscountValue;

            var excelReport = GetQuery(chosenOne, startDay, endDay);
            var er = excelReport.ReportDataList;
            int counter = er.Count();

            string filePath = "Templates\\ReportTemplate.xlsx";
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet("Sheet1");

            string newFilePath = er.ElementAt(0).Name + "_" + startDay.ToString("MMMM") +
                "_" + startDay.ToString("yyyy");

            ws.Cell(2, 1).Value =
                "Отчет на " + er.ElementAt(0).Name + " за периода от " +
                startDay.ToString("dd.MM.yyyy") + " до " + endDay.ToString("dd.MM.yyyy");
            int currentRow = 6;
            double sum = 0;
            double monthTax = 0;

            for (int i = 0; i < counter; i++)
            {
                ws.Cell(currentRow, 1).Value = er.ElementAt(i).Day;
                ws.Cell(currentRow, 2).Value = "Почистване";
                ws.Cell(currentRow, 3).Value = "1";
                ws.Cell(currentRow, 4).Value = (er.ElementAt(i).Tax != 0 ? er.ElementAt(i).Tax.Value.ToString("0.00") : string.Empty);
                ws.Cell(currentRow, 5).Value = (er.ElementAt(i).Tax != 0 ? er.ElementAt(i).Tax.Value.ToString("0.00") : string.Empty);
                sum = sum + er.ElementAt(i).Tax.Value;
                if (er.ElementAt(i).AddOn != string.Empty)
                {
                    currentRow++;
                    ws.Cell(currentRow, 2).Value = "Допънителни дейности:";
                    currentRow++;
                    ws.Cell(currentRow, 2).Value = er.ElementAt(i).AddOn;
                }
                if (er.ElementAt(i).ExtraNameList != string.Empty)
                {
                    currentRow++;
                    ws.Cell(currentRow, 2).Value = er.ElementAt(i).ExtraNameList;
                    ws.Cell(currentRow, 3).Value = er.ElementAt(i).ExtraCountList;
                    ws.Cell(currentRow, 4).Value = er.ElementAt(i).ExtraPriceList;
                    ws.Cell(currentRow, 5).Value = er.ElementAt(i).ExtraOveralList;
                    sum = sum + er.ElementAt(i).ExtraFinal.Value;
                }
                if (er.ElementAt(i).ConNameList != string.Empty)
                {
                    currentRow++;
                    ws.Cell(currentRow, 2).Value = er.ElementAt(i).ConNameList;
                    ws.Cell(currentRow, 3).Value = er.ElementAt(i).ConCountList;
                    ws.Cell(currentRow, 4).Value = er.ElementAt(i).ConPriceList;
                    ws.Cell(currentRow, 5).Value = er.ElementAt(i).ConOveralList;
                    sum = sum + er.ElementAt(i).ConFinal.Value;
                }
                currentRow = currentRow + 2;
                monthTax = er.ElementAt(i).MonthTax.Value;
            }
            if (discount != string.Empty)
            {
                ws.Cell(currentRow, 2).Value = discount;
                ws.Cell(currentRow, 5).Value = (0 - discountValue).ToString("0.00");
                sum -= discountValue;
                currentRow += 2;
            }

            ws.Range(6, 1, currentRow, 5).Style.Alignment.WrapText = true;

            if (monthTax != 0)
            {
                ws.Cell(currentRow, 4).Value = "Абонамент:";
                ws.Cell(currentRow, 5).Value = monthTax.ToString("0.00");
                sum = sum + monthTax;
                currentRow++;
            }
            
                
            ws.Range(currentRow, 3, currentRow, 4).Merge();
            ws.Cell(currentRow, 3).Value = "Общa стойност (без ДДС):";
            ws.Cell(currentRow, 5).Value = sum.ToString("0.00");
            ws.Cell(currentRow, 3).Style.Font.Bold = true;
            ws.Cell(currentRow, 5).Style.Font.Bold = true;
            ws.Range(6, 1, currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range(6, 1, currentRow, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    newFilePath);
            }
        }

        //---------------------------EXCEL Faktura-----------------------------------
        [Authorize (Roles = "Admin, StaffOperator")]
        
        public FileResult ExcelDDSInvoice(DateTime aDate, int inNumber, double inPrice)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var inDDS = _ctx.InvoiceDDS.Where(c => c.InvoiceNumber == inNumber).FirstOrDefault();
                Client cl = client.GetByIdAsync(inDDS.ClientId).Result;
                double dds = Math.Round(inPrice * 0.2, 2);
                string priceByWords = Convertor.ConvertToLetter(inPrice+dds);
                string filePath = "Templates\\InvoiceDDS.xlsx";
                var wb = new XLWorkbook(filePath);
                var ws = wb.Worksheet("Sheet1");
                string newFilePath = inNumber +  "_" + cl.Name + "_" + startDay.ToString("MMMM") +
                "_" + startDay.ToString("yyyy");

                ws.Cell(2,1).Value += " " + cl.Name;
                ws.Cell(3, 1).Value += " " + cl.Ein;
                ws.Cell(4, 1).Value += cl.Ein;
                ws.Cell(7, 1).Value += " " + cl.City;
                ws.Cell(8, 1).Value += " " + cl.Address;
                ws.Cell(9, 1).Value += " " + cl.Mol;
                ws.Cell(10, 1).Value += " " + inNumber.ToString("D10");
                ws.Cell(10, 5).Value += " " + aDate.ToShortDateString();
                ws.Cell(14, 10).Value = inPrice.ToString("0.00");
                ws.Cell(14, 11).Value = inPrice.ToString("0.00");
                ws.Cell(17, 2).Value = priceByWords;
                ws.Cell(17, 11).Value = inPrice.ToString("0.00");
                ws.Cell(18, 11).Value = dds.ToString("0.00");
                ws.Cell(19, 11).Value = (inPrice + dds).ToString("0.00");
                ws.Cell(21, 1).Value += " " + cl.Recipient;

                wb.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    newFilePath);
            }
        }
        [Authorize (Roles = "Admin, StaffOperator")]
        public FileResult ExcelSimpleInvoice(DateTime aDate, int inNumber, double inPrice)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var inSimple = _ctx.InvoiceSimple.Where(c => c.InvoiceNumber == inNumber).FirstOrDefault();
                Client cl = client.GetByIdAsync(inSimple.ClientId).Result;
                //double dds = Math.Round(inPrice * 0.2, 2);
                string priceByWords = Convertor.ConvertToLetter(inPrice);
                string filePath = "Templates\\InvoiceSimple.xlsx";
                var wb = new XLWorkbook(filePath);
                var ws = wb.Worksheet("Sheet1");
                string newFilePath = inNumber + "_" + cl.Name + "_" + startDay.ToString("MMMM") +
                "_" + startDay.ToString("yyyy");

                ws.Cell(2, 1).Value += " " + cl.Name;
                //ws.Cell(3, 1).Value += " " + cl.Ein;
                //ws.Cell(4, 1).Value += " " + "BG" + cl.Ein;
                //ws.Cell(7, 1).Value += " " + cl.City;
                //ws.Cell(8, 1).Value += " " + cl.Address;
                //ws.Cell(9, 1).Value += " " + cl.Mol;
                ws.Cell(9, 1).Value += " " + inNumber.ToString();
                ws.Cell(9, 5).Value += " " + aDate.ToShortDateString();
                ws.Cell(13, 10).Value = inPrice.ToString("0.00");
                ws.Cell(13, 11).Value = inPrice.ToString("0.00");
                ws.Cell(18, 2).Value = priceByWords;
                ws.Cell(18, 11).Value = inPrice.ToString("0.00");
                //ws.Cell(18, 11).Value = dds.ToString("0.00");
                ws.Cell(20, 11).Value = (inPrice).ToString("0.00");
                ws.Cell(22, 1).Value += " " + cl.Recipient;

                wb.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    newFilePath);
            }
        }
        //Helper functions
        private void LoadViewBag()//preparing viewbag
        {
            ViewBag.Client = client.GetAllActiveAsync().Result.OrderBy(t => t.Id);
            ViewBag.Staff = staff.GetAllAsync().Result;
            ViewBag.AddOn = addOn.GetAllAsync().Result;
        }

        //-------------------------------Queryyyyyyyyyyyyy---------------------------------
        [Authorize (Roles = "Admin, StaffOperator")]
        public ReportVM GetQuery(int chosenOne, DateTime start, DateTime end)//query for join tables
        {
            string ConsList = string.Empty;
            string ConsCountList = string.Empty;
            string ConsPriceList = string.Empty;
            string ConsOveralList = string.Empty;
            string ExtraList = string.Empty;
            string ExtraCountList = string.Empty;
            string ExtraPriceList = string.Empty;
            string ExtraOveralList = string.Empty;
            double ConsFinalPrice = 0;
            double ExtraFinalPrice = 0;
            List<ReportClassForVM> query = new List<ReportClassForVM>();

            var vc = _ctx.Visite.Include(c => c.Client).Where(v => v.ClientId == chosenOne).
                Where(v => v.Day.Value >= startDay && endDay >= v.Day.Value).OrderBy(v => v.Day.Value);

            var dc = _ctx.Deliveries.Include(c => c.Consumative).Where(v => v.ClientId == chosenOne).
                Where(v => v.Day.Value >= startDay && endDay >= v.Day.Value).OrderBy(v => v.Day.Value);

            var pc = _ctx.ProcessedService.Include(c => c.ExtraService).Where(v => v.ClientId == chosenOne).
               Where(v => v.Day.Value >= startDay && endDay >= v.Day.Value).OrderBy(v => v.Day.Value);

            for (int i = 0; i < vc.Count(); i++)
            {
                ConsList = ConsCountList = ConsPriceList = ConsOveralList = string.Empty;
                ExtraList = ExtraCountList = ExtraPriceList = ExtraOveralList = string.Empty;
                ConsFinalPrice = 0;
                ExtraFinalPrice = 0;

                for (int j = 0; j < dc.Count(); j++)
                {
                    if (dc.ElementAt(j).Day == vc.ElementAt(i).Day && dc.ElementAt(j).ConsCount != 0)
                    {
                        ConsList = ConsList + dc.ElementAt(j).Consumative.Name + "\n";
                        ConsCountList = ConsCountList + dc.ElementAt(j).ConsCount.ToString() + "\n";
                        ConsPriceList = ConsPriceList + dc.ElementAt(j).Consumative.Price.Value.ToString("0.00") + "\n";
                        ConsOveralList = ConsOveralList + (dc.ElementAt(j).ConsCount.Value * dc.ElementAt(j).Consumative.Price.Value).ToString("0.00") + '\n';
                        ConsFinalPrice = ConsFinalPrice + dc.ElementAt(j).ConsCount.Value * dc.ElementAt(j).Consumative.Price.Value;
                    }
                }
                foreach (var entry in pc)
                {
                    if (entry.Day == vc.ElementAt(i).Day && entry.ExtraCount != 0)
                    {
                        ExtraList = ExtraList + entry.ExtraService.Name + "\n";
                        ExtraCountList = ExtraCountList + entry.ExtraCount.ToString() + "\n";
                        ExtraPriceList = ExtraPriceList + entry.ExtraService.ExtraPrice.Value.ToString("0.00") + "\n";
                        ExtraOveralList = ExtraOveralList + (entry.ExtraCount * entry.ExtraService.ExtraPrice.Value).Value.ToString("0.00") + "\n";
                        ExtraFinalPrice = ExtraFinalPrice + entry.ExtraCount.Value * entry.ExtraService.ExtraPrice.Value;
                    }
                }
                var lm = new ReportClassForVM();
                lm.vId = vc.ElementAt(i).Id;
                lm.Name = vc.ElementAt(i).Client.Name;
                lm.Day = vc.ElementAt(i).Day;
                lm.Staff = vc.ElementAt(i).Staff;
                lm.AddOn = vc.ElementAt(i).AddOn;
                lm.ConNameList = ConsList.TrimEnd('\n');
                lm.ConCountList = ConsCountList.TrimEnd('\n');
                lm.ConPriceList = ConsPriceList.TrimEnd('\n');
                lm.ConOveralList = ConsOveralList.TrimEnd('\n');
                lm.ConFinal = ConsFinalPrice;
                lm.ExtraNameList = ExtraList.TrimEnd('\n');
                lm.ExtraCountList = ExtraCountList.TrimEnd('\n');
                lm.ExtraPriceList = ExtraPriceList.TrimEnd('\n');
                lm.ExtraOveralList = ExtraOveralList.TrimEnd('\n');
                lm.ExtraFinal = ExtraFinalPrice;
                if (vc.ElementAt(i).OnMonth == false)
                {
                    lm.Tax = vc.ElementAt(i).Tax;
                    lm.MonthTax = 0;
                }
                else
                {
                    lm.Tax = 0;
                    lm.MonthTax = vc.ElementAt(i).Tax;
                }
                lm.Remark = vc.ElementAt(i).Remark;

                query.Add(lm);
            }
            var model = new ReportVM { ReportDataList = query.AsEnumerable() };

            return (model);
        }
    }
}



