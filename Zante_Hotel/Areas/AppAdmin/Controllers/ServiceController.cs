
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Models;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Service> services = await _context.Services.Include(c => c.Rooms).ThenInclude(r=>r.Room).ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM serviceVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Services.Any(c => c.Name.Trim().ToLower() == serviceVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda service artiq movcuddur");
                return View();
            }
            Service service = new Service
            {
                Name = serviceVM.Name
            };
            if (serviceVM.Icon != null) service.Icon = serviceVM.Icon;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Service existed = await _context.Services.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateServiceVM serviceVM = new UpdateServiceVM
            {
                Name = existed.Name,
                Icon=existed.Icon
            };
            return View(serviceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateServiceVM serviceVM)
        {
            if (id == null) return BadRequest();
            Service existed = await _context.Services.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (serviceVM.Icon != null && existed.Icon != serviceVM.Icon) existed.Icon = serviceVM.Icon;
            if (existed.Name != null && existed.Name != serviceVM.Name && !await _context.Services.AnyAsync(c => c.Name.Trim().ToLower() == serviceVM.Name.Trim().ToLower() && c.Id != id))
                existed.Name = serviceVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Service existed = await _context.Services.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Services.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

