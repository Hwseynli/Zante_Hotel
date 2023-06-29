using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class SliderController : Controller
    {
        private string urlimgroot = @"assets/assets/images/slider";
        private string urlmp4root = @"assets/assets/videos";
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _dbContext = context;
            _env = env;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _dbContext.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM sliderVM)
        {
            if (!ModelState.IsValid) return View();
            Slider slider = new Slider
            {
                Title = sliderVM.Title,
                SubTitle = sliderVM.SubTitle,
                ButtonTitle = sliderVM.ButtonTitle,
                VideoUrl=sliderVM.VideoUrl
            };
            if (sliderVM.Photo != null)
            {
                if (!sliderVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                    return View();
                }
                if (!sliderVM.Photo.CheckFileSize(2000))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi uygun deyil");
                    return View();
                }
                slider.ImageUrl = await sliderVM.Photo.CreateFileAsync(_env.WebRootPath, urlimgroot);
            }

            await _dbContext.Sliders.AddAsync(slider);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Slider slide = await _dbContext.Sliders.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (slide == null) return NotFound();
            UpdateSliderVM sliderVM = new UpdateSliderVM
            {
                ImageUrl = slide.ImageUrl,
                VideoUrl = slide.VideoUrl,
                SubTitle = slide.SubTitle,
                Title = slide.Title,
                ButtonTitle = slide.ButtonTitle,
            };
            return View(sliderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateSliderVM sliderVM)
        {
            if (id == null) return BadRequest();
            Slider slide = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (sliderVM.ButtonTitle != null && sliderVM.ButtonTitle != slide.ButtonTitle) slide.ButtonTitle = sliderVM.ButtonTitle;
            if (sliderVM.Title != null && sliderVM.Title != slide.Title) slide.Title = sliderVM.Title;
            if (sliderVM.VideoUrl != null && sliderVM.VideoUrl != slide.VideoUrl) slide.VideoUrl = sliderVM.VideoUrl;
            if (sliderVM.SubTitle != null && sliderVM.SubTitle != slide.SubTitle) slide.SubTitle = sliderVM.SubTitle;
            if (sliderVM.Photo != null)
            {
                if (!sliderVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                    return View();
                }
                if (!sliderVM.Photo.CheckFileSize(20000))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi uygun deyil");
                    return View();
                }
                slide.ImageUrl.DeleteFile(_env.WebRootPath, urlimgroot);
                slide.ImageUrl = await sliderVM.Photo.CreateFileAsync(_env.WebRootPath, urlimgroot);
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Slider slide = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            slide.ImageUrl.DeleteFile(_env.WebRootPath, urlimgroot);
            _dbContext.Sliders.Remove(slide);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

