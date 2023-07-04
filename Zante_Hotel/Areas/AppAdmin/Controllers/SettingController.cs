using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SettingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _dbContext.Settings.ToListAsync();
            return View(settings);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSettingVM settingVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _dbContext.Settings.Any(c => c.Key.Trim().ToLower() == settingVM.Key.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Key", "Bu adda Setting artiq movcuddur");
                return View();
            }
           
            bool valueresult = _dbContext.Settings.Any(c => c.Key.Trim().ToLower() == settingVM.Key.Trim().ToLower());
            if (valueresult)
            {
                ModelState.AddModelError("Value", "Bu value li Setting artiq movcuddur");
                return View();
            }
            Setting setting = new Setting
            {
                Key = settingVM.Key,
                Value=settingVM.Value
            };
            await _dbContext.Settings.AddAsync(setting);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest(ModelState);
            Setting setting = await _dbContext.Settings.FirstOrDefaultAsync(p => p.Id == id);
            if (setting == null) return NotFound();
            UpdateSettingVM updateSetting = new UpdateSettingVM
            {
                Key = setting.Key,
                Value = setting.Value,
            };
            return View(updateSetting);
        }
        public async Task<IActionResult> Update(Guid? id, UpdateSettingVM settingVM)
        {
            if (id == null) return BadRequest(ModelState);
            Setting existed = await _dbContext.Settings.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid) return View(existed);
            existed.Value = settingVM.Value;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

