using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _dbContext;
        public ReservationController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Reservation> reservations = await _dbContext.Reservations.Include(i => i.Room).ToListAsync();
            return View(reservations);
        }
    }
}