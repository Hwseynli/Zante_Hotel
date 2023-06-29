
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _dbContext;

        public RoomController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> RoomList()
        {
            HomeVM homeVM = new HomeVM
            {
                Rooms = await _dbContext.Rooms.Include(r=>r.Category).Include(r => r.View).Include(r => r.Images).Include(r => r.Services).ToListAsync()
            };
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(Guid? id)
        {
            if (id is null) return View();
            Room room = await _dbContext.Rooms.Where(r => r.Id == id).Include(r => r.Category).Include(r => r.View).Include(r => r.Images).Include(r => r.Services).ThenInclude(s=>s.Service).FirstOrDefaultAsync();
            if (room is null) return View();
            HomeVM homeVM = new HomeVM
            {
                Room = room
            };
            return View(homeVM);
        }
    }
}

