﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Utilities.Exceptions;

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
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) throw new BadRequestException();
            Reservation existed = await _dbContext.Reservations.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) throw new NotFoundException();
            _dbContext.Reservations.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}