using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Utilities.Exceptions;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class FoodController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;

        public FoodController(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Food> foods = await _dbContext.Foods.Include(f => f.RestFoods).ToListAsync();
            return View(foods);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodVM foodVM)
        {
            if (!ModelState.IsValid) return View();
            Food food = new Food
            {
                About = foodVM.About,
                Name = foodVM.Name,
            };
            if (foodVM.Photo != null)
            {
                if (!foodVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!foodVM.Photo.CheckFileSize(20000))
                {
                    ModelState.AddModelError("Photo", "File olcusu uygun deyil");
                    return View();
                }
                food.ImageUrl = await foodVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/foods");
            }
            if (foodVM.Price < 0)
            {
                ModelState.AddModelError("Price","Duzgun qiymat daxil edin");
                return View();
            }
            food.Price = foodVM.Price;
            await _dbContext.Foods.AddAsync(food);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) throw new BadRequestException();
            Food food = await _dbContext.Foods.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (food == null) throw new NotFoundException();
            UpdateFoodVM foodVM = new UpdateFoodVM
            {
                About = food.About,
                Price = food.Price,
                ImgUrl = food.ImageUrl,
                Name = food.Name
            };
            return View(foodVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id,UpdateFoodVM foodVM)
        {
            if (id == null) throw new BadRequestException();
            Food existed = await _dbContext.Foods.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            if (!ModelState.IsValid) return View();
            if (foodVM.Photo != null)
            {
                if (!foodVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!foodVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200 kb den cox olmamalidir");
                    return View();
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/foods");
                existed.ImageUrl = await foodVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/assets/images/foods");
            }
            if (foodVM.Name != null && foodVM.Name != existed.Name && !(_dbContext.Blogs.Any(b => b.Name == foodVM.Name))) existed.Name = foodVM.Name;
            if (foodVM.About != null && foodVM.About != existed.About && !(_dbContext.Foods.Any(b => b.Name == foodVM.Name))) existed.About = foodVM.About;
            if (foodVM.Price > 0 && foodVM.Price != existed.Price) existed.Price = foodVM.Price;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) throw new BadRequestException();
            Food food = await _dbContext.Foods.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (food == null) throw new NotFoundException();
            if (!await _dbContext.Foods.AnyAsync(f=>f.Id==food.Id))
            {
                ModelState.AddModelError(string.Empty, "Bele bir food yoxdur");
                return View();
            }
            if (food.ImageUrl != null)
            {
                food.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/foods");
                _dbContext.Foods.Remove(food);
            }
            _dbContext.Foods.Remove(food);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

