using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReservationController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _context.Rooms.Include(r => r.Category).ToListAsync();
            ReservationVM reservationVM = new ReservationVM();
            return View(reservationVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservationVM reservationVM)
        {
            ViewBag.Rooms = await _context.Rooms.ToListAsync();
            if (!ModelState.IsValid) return View(reservationVM);
            Room room = await _context.Rooms.Where(c=>c.Id==reservationVM.RoomId).Include(r=>r.ReservationsDate).FirstOrDefaultAsync();
            if (room == null)
            {
                ModelState.AddModelError("RoomId","Bu id li room tapilmadi");
                return View(reservationVM);
            }
            List<Reservation> existed = room.ReservationsDate.ToList();
            if(reservationVM.ArrivalDateTime.Date>reservationVM.DepartureDateTime.Date)
            {
                ModelState.AddModelError("DepartureDateTime", "DepartureDate ArrivalDate dan boyuk olmalidir");
                return View(reservationVM);
            }
            if (reservationVM.ArrivalDateTime.TimeOfDay > reservationVM.DepartureDateTime.TimeOfDay)
            {
                ModelState.AddModelError("DepartureDateTime", "DepartureTime ArrivalTime dan boyuk olmalidir");
                return View(reservationVM);
            }
            if (reservationVM.ArrivalDateTime < DateTime.Now)
            {
                ModelState.AddModelError("ArrivalDate", "Reservasiya gelecekdeki bir zaman ucun goturulmelidir");
                return View(reservationVM);
            }
            if ((reservationVM.DepartureDateTime < reservationVM.ArrivalDateTime.AddHours(1) || reservationVM.DepartureDateTime > reservationVM.ArrivalDateTime.AddYears(1))&&reservationVM.DepartureDateTime.Year- reservationVM.ArrivalDateTime.Year>0)
            {
                ModelState.AddModelError("ArrivalTime", "Departure time ile Arrival time en az bir saat mesafe olmalidir.Yeni reservasiya min 1 saatliqdir max 1 illik");
                return View(reservationVM);
            }
            foreach (Reservation item in existed)
            {
                if (item != null)
                {
                    if (item.ArrivalDateTime.Year == reservationVM.ArrivalDateTime.Year)
                    {
                        int month = reservationVM.DepartureDateTime.Month - reservationVM.ArrivalDateTime.Month;
                        int monthcount = 0;
                        for (int i = 1; i < month + 1; i++)
                        {
                            if (item.ArrivalDateTime.Month == reservationVM.ArrivalDateTime.AddMonths(i).Month)
                            {
                                monthcount++;
                                int day = reservationVM.DepartureDateTime.Day - reservationVM.ArrivalDateTime.Day;
                                int counter = 0;
                                for (int j = 1; j < day + 1; j++)
                                {
                                    if (item.DepartureDateTime.Day == reservationVM.ArrivalDateTime.AddDays(i).Day)
                                    {
                                        counter++;
                                        if (item.ArrivalDateTime.Hour == reservationVM.ArrivalDateTime.Hour+j)
                                        {
                                            ModelState.AddModelError("ArrivalTime", $"Bu saatda rezerv olunub");
                                            return View(reservationVM);
                                        }
                                        int time = reservationVM.DepartureDateTime.Hour - reservationVM.ArrivalDateTime.Hour;
                                        int count = 0;
                                        for (int y = 1; y < time + 1; y++)
                                        {
                                            if (item.DepartureDateTime.Hour == reservationVM.ArrivalDateTime.AddHours(i).Hour)
                                            {
                                                count++;
                                            }
                                        }
                                        if (count > 0)
                                        {
                                            ModelState.AddModelError("DepartureTime", $"Bu saat araliginda  rezerv olunub");
                                            return View(reservationVM);
                                        }
                                    }
                                }
                                if (counter > 0)
                                {
                                    ModelState.AddModelError("DepartureDate", $"Bu gun araliginda rezerv olunub her hansi bir gun ve ya gunler");
                                    return View(reservationVM);
                                }
                            }
                        }
                        if (monthcount>0)
                        {
                            ModelState.AddModelError("DepartureDate", $"Bu ay araliginda  rezerv olunub her hansi bir ay ve ya gun");
                            return View(reservationVM);
                        }  
                    }
                }
            }
            Reservation reservation = _mapper.Map<Reservation>(reservationVM);
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
