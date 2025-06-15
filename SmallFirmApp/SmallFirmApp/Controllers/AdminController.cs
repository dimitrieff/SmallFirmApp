using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using SmallFirmApp.Constants;
using SmallFirmApp.Data;
using SmallFirmApp.Models.ProgramModels;
using SmallFirmApp.Repositories;

namespace SmallFirmApp.Controllers
{
    public class AdminController : Controller
    {
        private static int currentClientId;
        private static DateTime currentDay;
        private static DateTime startDay;
        private static DateTime endDay;
        private static int chosenOne;
        private static int visiteID;
        private static DateTime editDay;
        private static string callAct;
        private static string callCtr;
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

        public AdminController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
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

            _ctx = ctx;
            _userManager = userManager;
        }

        //---------------------Main--------------------------------
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        //-------------------------AddOns----------------------------

        [Authorize]
        public async Task<IActionResult> SetAddon()
        {
            var add = await addOn.GetAllAsync();
            return View(add);
        }

        [Authorize]
        public IActionResult CreateAddon()
        {
            return View(new AddOn());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAddon(AddOn a)
        {
            await addOn.AddAsync(a);
            return RedirectToAction("SetAddon");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAddon(int id, string name)
        {
            var add = await addOn.GetByIdAsync(id);
            add.Name = name;
            await addOn.UpdateAsync(add);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAddon(int id)
        {
            var add = await addOn.GetByIdAsync(id);
            if (add == null)
            {
                return NotFound();
            }
            await addOn.DeleteAsync(id);
            return Ok();
        }


        //--------------------CONSUMATIVES---------------------------
        [Authorize]
        public async Task<IActionResult> SetConsumatives()
        {
            var con = consumative.GetAllActiveAsync().Result.OrderBy(c => c.Name);
            //con.OrderBy(c => c.isActive);
            return View(con);
        }

        [Authorize]
        public IActionResult CreateConsumative(string act, string ctr)
        {
            callAct = act;
            callCtr = ctr;

            return View("~/Views/Admin/CreateConsumative.cshtml",new Consumative());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateConsumative(Consumative c)
        {
            await consumative.AddAsync(c);
            return RedirectToAction(callAct, callCtr);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditConsumative(int id, string name, double price)
        {
            var conOld = await consumative.GetByIdAsync(id);
            conOld.isActive = 0;
            //conOld.date = DateTime.Now;
            await consumative.UpdateAsync(conOld);

            Consumative conNew = new Consumative
            {
                Name = name,
                Price = price,
            };
            await consumative.AddAsync(conNew);
            
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteConsumative(int id)
        {
            var con = await consumative.GetByIdAsync(id);
            con.isActive = 0;
            con.date = DateTime.Now;
            await consumative.UpdateAsync(con);

            return Ok();
        }

        [Authorize]
        
        public async Task<IActionResult> ListAllConsumatives()
        {
            //var con = consumative.GetAllAsync().Result.OrderBy(c => c.Name);
            //con.OrderBy(c => c.isActive);
            var c = await consumative.GetAllAsync();
            var con = c.OrderBy(c => c.Name);
            return View(con);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DbEditConsumative(int id, string name, double price, int isActive)
        {
            var con = await consumative.GetByIdAsync(id);
            con.isActive = isActive;
            con.Name = name;
            con.Price = price;
            con.date = DateTime.Now;
            await consumative.UpdateAsync(con);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DbDeleteConsumative(int id)
        {
            var con = await consumative.GetByIdAsync(id);
            try
            {
                await consumative.DeleteAsync(id);
            }
            catch
            {
                throw new Exception("Записът е свързан с друга база");
            }
            
            return Ok();
        }

        //--------------------------------ExtraServices---------------------------

        [Authorize]
        public async Task<IActionResult> SetServices()
        {
            var ex = extraService.GetAllActiveAsync().Result.OrderBy(c => c.Name); ;
            return View(ex);
        }

        [Authorize]
        public IActionResult CreateService()
        {
            return View(new ExtraService());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateService(ExtraService e)
        {
            await extraService.AddAsync(e);
            return RedirectToAction("SetServices");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditService(int id, string name, double price)
        {
            var exOld = await extraService.GetByIdAsync(id);
            exOld.isActive = 0;
            await extraService.UpdateAsync(exOld);
            ExtraService exNew = new ExtraService
            {
                Name = name,
                ExtraPrice = price
            };
            await extraService.AddAsync(exNew);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var ex = await extraService.GetByIdAsync(id);
            ex.isActive = 0;
            ex.date = DateTime.Now;
            await extraService.UpdateAsync(ex);
            return Ok();
        }

        public async Task<IActionResult> ListAllServices()
        {
            var e = await extraService.GetAllAsync();
            var ex = e.OrderBy(e => e.Name);
            return View(ex);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DbEditService(int id, string name, double price, int isActive)
        {
            var ex = await extraService.GetByIdAsync(id);
            ex.isActive = isActive;
            ex.Name = name;
            ex.ExtraPrice = price;
            ex.date = DateTime.Now;
            await extraService.UpdateAsync(ex);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DbDeleteService(int id)
        {
            var con = await extraService.GetByIdAsync(id);
            try
            {
                await extraService.DeleteAsync(id);
            }
            catch
            {
                throw new Exception("Записът е свързан с друга база");
            }
            return Ok();
        }

        //-----------------------------STAFF---------------------------------------------

        [Authorize]
        public async Task<IActionResult> SetStaff()
        {
            var s = await staff.GetAllActiveAsync();
            return View(s);
        }

        [Authorize]
        public IActionResult CreateStaff()
        {
            return View(new Staff());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStaff(Staff s)
        {
            await staff.AddAsync(s);
            return RedirectToAction("SetStaff");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditStaff(int id, string name, int isActive)
        {
            var staffOld = await staff.GetByIdAsync(id);
            staffOld.isActive = isActive;
            staffOld.Name = name;
            await staff.UpdateAsync(staffOld);
            //Staff staffNew = new Staff
            //{
            //    Name = name,
            //    date = DateTime.Now,
            //};
            //await staff.AddAsync(staffNew);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var s = await staff.GetByIdAsync(id);
            s.isActive = 0;
            s.date = DateTime.Now;
            await staff.UpdateAsync(s);
            return Ok();
        }
        [Authorize]
        public async Task<IActionResult> ListAllStaff()
        {
            var s = await staff.GetAllAsync();
            var st = s.OrderBy(s => s.Name);
            return View(st);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DbEditStaff(int id, string name, int isActive)
        {
            var s = await staff.GetByIdAsync(id);
            s.isActive = isActive;
            s.Name = name;
            s.date = DateTime.Now;
            await staff.UpdateAsync(s);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DbDeleteStaff(int id)
        {
            var con = await staff.GetByIdAsync(id);
            try
            {
                await staff.DeleteAsync(id);
            }
            catch
            {
                throw new Exception("Записът е свързан с друга база");
            }
            return Ok();
        }

        //-----------------------------CLIENT---------------------------------------------

        [Authorize]
        public async Task<IActionResult> SetClient()
        {
            var cl = (await client.GetAllActiveAsync()).OrderBy(c => c.Name);
            return View(cl);
        }

        [Authorize]
        public async Task<IActionResult> ListAllClient()
        {
            var cl = (await client.GetAllAsync()).OrderBy(c => c.Name);
            return View(cl);
        }

        [Authorize]
        public IActionResult CreateClient()
        {
            return View(new Client());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateClient(Client c)
        {
            await client.AddAsync(c);
            return RedirectToAction("SetClient");
        }

        [Authorize]
        public async Task<IActionResult> DbEditClient(int id)
        {
            var cl = await client.GetByIdAsync(id);
            return View(cl);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DbEditClient(Client c)
        {
            await client.UpdateAsync(c);
            return RedirectToAction("SetClient");
        }
    }
}

