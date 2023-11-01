using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zante_Hotel.Utilities.Exceptions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class ContactusController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ContactusController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Message> comments = await _dbContext.Messages.ToListAsync();
            return View(comments);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) throw new BadRequestException();
            Message existed = await _dbContext.Messages.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException();
            _dbContext.Messages.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}