using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class ViewController : Controller
    {
        private readonly AppDbContext _context;
        public ViewController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<View> views = await _context.Views.Include(c => c.Rooms).ToListAsync();
            return View(views);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewVM viewVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Views.Any(c => c.Name.Trim().ToLower() == viewVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda categoriya artiq movcuddur");
                return View();
            }
            View view = new View
            {
                Name = viewVM.Name
            };
            await _context.Views.AddAsync(view);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Update(int? id)
        //{
        //    if (id == null || id < 1) return BadRequest();
        //    View existed = await _context.Views.FirstOrDefaultAsync(c => c.Id == id);
        //    if (existed == null) return NotFound();
        //    return View(existed);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(int? id, UpdateViewVM viewVM)
        //{
        //    if (id == null || id < 1) return BadRequest();
        //    View existed = await _context.Views.FirstOrDefaultAsync(c => c.Id == id);
        //    if (existed == null) return NotFound();
        //    if (existed.Name == viewVM.Name)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return View(existed);
        //    }
        //    bool result = _context.Views.Any(c => c.Name.Trim().ToLower() == viewVM.Name.Trim().ToLower() && c.Id != id);
        //    if (result)
        //    {
        //        ModelState.AddModelError("Name", "Bu adda view artiq movcuddur");
        //        return View(existed);
        //    }
        //    existed.Name = viewVM.Name;
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || id < 1) return BadRequest();
        //    View existed = await _context.Views.FirstOrDefaultAsync(c => c.Id == id);
        //    if (existed == null) return NotFound();
        //    _context.Views.Remove(existed);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

