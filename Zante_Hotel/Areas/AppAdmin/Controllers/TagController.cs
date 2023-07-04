
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _context.Tags.Include(c => c.Blogs).ToListAsync();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Tags.Any(c => c.Name.Trim().ToLower() == tagVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda TAG artiq movcuddur");
                return View();
            }
            Tag tag = new Tag
            {
                Name = tagVM.Name
            };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateTagVM tagVM)
        {
            if (id == null) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (existed.Name == tagVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            bool result = _context.Tags.Any(c => c.Name.Trim().ToLower() == tagVM.Name.Trim().ToLower() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda tag artiq movcuddur");
                return View(existed);
            }
            existed.Name = tagVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Tags.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

