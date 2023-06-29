﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class GalleryController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public GalleryController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Gallery> galaries = await _dbContext.Galleries.ToListAsync();
            return View(galaries);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGalleryVM galleryVM)
        {
            if (!ModelState.IsValid) return View();
            Gallery gallery = new Gallery();
            if (galleryVM.Photo != null)
            {
                if (!galleryVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!galleryVM.Photo.CheckFileSize(20000))
                {
                    ModelState.AddModelError("Photo", "File olcusu uygun deyil");
                    return View();
                }
                gallery.Url = await galleryVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/gallery");
            }
            await _dbContext.Galleries.AddAsync(gallery);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id is null) return BadRequest();
            Gallery gallery = await _dbContext.Galleries.FirstOrDefaultAsync(g => g.Id == id);
            if (gallery is null) return NotFound();
            UpdateGalleryVM galleryVM = new UpdateGalleryVM
            {
                Url = gallery.Url
            };
            return View(galleryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateGalleryVM galleryVM)
        {
            if (id is null) return BadRequest();
            Gallery gallery = await _dbContext.Galleries.FirstOrDefaultAsync(g => g.Id == id);
            if (gallery is null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (galleryVM.Photo != null)
            {
                if (!galleryVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!galleryVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200 kb den cox olmamalidir");
                    return View();
                }
                gallery.Url.DeleteFile(_env.WebRootPath, @"assets/assets/images/gallery");
                gallery.Url = await galleryVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/assets/images/gallery");
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) return BadRequest();
            Gallery gallery = await _dbContext.Galleries.FirstOrDefaultAsync(g => g.Id == id);
            if (gallery is null) return NotFound();
            if (gallery.Url != null)
            {
                gallery.Url.DeleteFile(_env.WebRootPath, @"assets/assets/images/gallery");
                _dbContext.Galleries.Remove(gallery);
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
