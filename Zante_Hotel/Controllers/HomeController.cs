using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;
    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {

        ICollection<Gallery> galleries = await _dbContext.Galleries.Take(12).ToListAsync();
        Slider slider = await _dbContext.Sliders.Where(s => s.VideoUrl != null).FirstOrDefaultAsync();
        ICollection<Room> rooms = await _dbContext.Rooms.Take(3).Include(r => r.Images.Where(ri => ri.IsPrimary)).Include(r => r.Category).Include(r => r.View).Include(r => r.Services).ToListAsync();
        HomeVM homeVM = new HomeVM
        {
            Rooms = rooms,
            Slider = slider,
            Galleries = galleries
        };
        return View(homeVM);
    }
    public async Task<IActionResult> ContactUs()
    {
        return View();
    }
}

