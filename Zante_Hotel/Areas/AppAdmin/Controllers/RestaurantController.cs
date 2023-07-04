using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;
        public RestaurantController(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            Hotel hotel = await _dbContext.Hotels.FirstOrDefaultAsync();
            ICollection<Restaurant> restaurants = await _dbContext.Restaurants.Where(r => r.HotelId == hotel.Id).Include(r => r.RestFoods).Include(r=>r.Hotel).Include(r => r.Images).ToListAsync();
            return View(restaurants);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Foods = await _dbContext.Foods.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantVM restaurantVM)
        {
            if (!ModelState.IsValid) return View();
            return View();
        }
    }
}

