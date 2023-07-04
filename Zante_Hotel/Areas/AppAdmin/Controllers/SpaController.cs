using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class SpaController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;
        public SpaController(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Spa> spas = await _dbContext.Spas.Include(s => s.Images).Include(s=>s.Hotel).ToListAsync();
            return View(spas);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSpaVM spaVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            if (!ModelState.IsValid) return View();
            bool resultcat = await _dbContext.Hotels.AnyAsync(c => c.Id == spaVM.HotelId);
            if (!resultcat)
            {
                ModelState.AddModelError("HotelId", "Bu id-li hotel movcud deyil");
                return View();
            }
            if (await _dbContext.Spas.AnyAsync(c => c.Name == spaVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli otaq artiq movcuddur");
                return View();
            }
            Spa spa = new Spa
            {
                Title=spaVM.Title,
                SubTitle=spaVM.SubTitle,
                Name = spaVM.Name,
                HotelId = spaVM.HotelId,
                Decription = spaVM.Decription,
                Images = new List<SpaImage>()
            };

            if (spaVM.MainPhoto != null)
            {
                if (!spaVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!spaVM.MainPhoto.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }

                spa.Images.Add(new SpaImage
                {
                    ImageUrl = await spaVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/spa"),
                    IsPrimary = true,
                    Spa = spa
                });
            }
            foreach (IFormFile photo in spaVM.Photos)
            {
                if (photo != null)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File tipi uygun deyil");
                        return View();
                    }
                    if (!photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photos", "File olcusu uygun deyil");
                        return View();
                    }
                    SpaImage addImage = new SpaImage
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/spa"),
                        Spa = spa,
                        IsPrimary = false
                    };
                    spa.Images.Add(addImage);
                }
            }
            await _dbContext.Spas.AddAsync(spa);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            if (id is null) return BadRequest();
            Spa existed = await _dbContext.Spas.Where(r => r.Id == id).Include(r => r.Images).Include(s=>s.Hotel).FirstOrDefaultAsync();
            if (existed == null) return NotFound();
            UpdateSpaVM spaVM = new UpdateSpaVM
            {
                HotelId = existed.HotelId,
                Name = existed.Name,
                SubTitle = existed.SubTitle,
                Decription = existed.Decription,
                Title = existed.Title
            };
            spaVM = MapImages(spaVM, existed);
            return View(spaVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id,UpdateSpaVM spaVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            if (id is null) return BadRequest();
            Spa existed = await _dbContext.Spas.Where(r => r.Id == id).Include(r => r.Images).Include(s=>s.Hotel).FirstOrDefaultAsync();
            if (existed == null) return NotFound();
            spaVM = MapImages(spaVM, existed);
            if (!ModelState.IsValid) return View(spaVM);
            if (await _dbContext.Hotels.AnyAsync(h => h.Id == spaVM.HotelId) && spaVM.HotelId != existed.HotelId) existed.HotelId = spaVM.HotelId;
            if (spaVM.Decription != null && spaVM.Decription != existed.Decription) existed.Decription = spaVM.Decription;
            if (spaVM.Name != null && spaVM.Name != existed.Name) existed.Name = spaVM.Name;
            if (spaVM.Title != null && spaVM.Title != existed.Title) existed.Title = spaVM.Title;
            if (spaVM.SubTitle != null && spaVM.SubTitle != existed.SubTitle) existed.SubTitle = spaVM.SubTitle;
            if (spaVM.MainPhoto != null)
            {
                if (!spaVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin novu uygun deyil");
                    return View(spaVM);
                }
                if (!spaVM.MainPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin olcusu uygun deyil");
                    return View(spaVM);
                }
                var mainImage = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/spa");
                existed.Images.Remove(mainImage);
                SpaImage spaImage = new SpaImage
                {
                    SpaId = existed.Id,
                    ImageUrl = await spaVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/spa"),
                    IsPrimary = true
                };
                existed.Images.Add(spaImage);
            }
            List<SpaImage> removeImageList = existed.Images.Where(pi => !spaVM.ImagesIds.Contains(pi.Id) && pi.IsPrimary == false).ToList();
            foreach (SpaImage rImage in removeImageList)
            {
                rImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/spa");
                existed.Images.Remove(rImage);
            }
            if (spaVM.Photos != null && spaVM.Photos.Count > 0)
            {
                foreach (var photo in spaVM.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File tipi uygun deyil");
                        return View();
                    }
                    if (!photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photos", "File olcusu uygun deyil");
                        return View();
                    }
                    existed.Images.Add(new SpaImage
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/spa"),
                        IsPrimary = false,
                        SpaId = existed.Id
                    });
                }
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) return BadRequest();
            Spa existed = await _dbContext.Spas.Include(r => r.Images).FirstOrDefaultAsync(r => r.Id == id);
            if (existed == null) return NotFound();
            if (existed.Images.Count > 0)
            {
                foreach (var item in existed.Images)
                {
                    item.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/spa");
                    _dbContext.SpaImages.Remove(item);
                }
            }
            _dbContext.Spas.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public UpdateSpaVM MapImages(UpdateSpaVM spaVM, Spa spa)
        {
            spaVM.ImageVMs = new List<SpaImageVM>();
            foreach (SpaImage image in spa.Images)
            {
                SpaImageVM imageVM = new SpaImageVM
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    IsPrimary = image.IsPrimary,
                };
                spaVM.ImageVMs.Add(imageVM);
            }
            return spaVM;
        }
    }
}

