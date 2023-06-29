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
        List<Room> rooms = await _dbContext.Rooms.Include(r => r.Images.Where(ri=>ri.IsPrimary)).Include(r => r.Category).Include(r => r.View).Include(r => r.Services).ToListAsync();
        HomeVM homeVM = new HomeVM
        {
            Rooms = rooms
        };
        return View(homeVM);
    }
   
}

