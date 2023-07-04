using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Utilities.Exceptions;

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
        public async Task<IActionResult> RoomList(int take=2,int page=1)
        {
            ViewBag.TotalPage = (int)Math.Ceiling((double)_dbContext.Rooms.Count() / take);
            ViewBag.CurrentPage=page;
            HomeVM homeVM = new HomeVM
            {
                Rooms = await _dbContext.Rooms.Include(r=>r.Category).Include(r => r.View).Include(r => r.Images.Where(i=>i.IsPrimary)).Include(r => r.Services).ThenInclude(rs=>rs.Service).Skip((page-1)*take).Take(take).ToListAsync()
            };

            return View(homeVM);
        }
        public async Task<IActionResult> Detail(Guid? id)
        {
            if (id is null) throw new BadRequestException();
            Room room = await _dbContext.Rooms.Where(r => r.Id == id).Include(r=>r.Hotel).Include(r => r.Category).Include(r => r.View).Include(r => r.Images).Include(r => r.Services).ThenInclude(s=>s.Service).FirstOrDefaultAsync();
            if (room is null) throw new NotFoundException();
            HomeVM homeVM = new HomeVM
            {
                Blogs=await _dbContext.Blogs.Where(b=>b.HotelId==room.HotelId).ToListAsync(),
                Room = room,
                Rooms = await _dbContext.Rooms.Where(r => (r.CategoryId == room.CategoryId || r.ViewId == room.ViewId) && r.Number != room.Number).Include(r => r.Images.Where(i => i.IsPrimary)).ToListAsync(),
            };
            return View(homeVM);
        }
      
    }
}

