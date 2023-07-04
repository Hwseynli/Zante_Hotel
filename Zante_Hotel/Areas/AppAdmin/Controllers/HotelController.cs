using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Utilities.Exceptions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class HotelController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public HotelController(AppDbContext context, IWebHostEnvironment env)
        {
            _dbContext = context;
            _env = env;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Hotel> hotels = await _dbContext.Hotels.Include(c => c.Rooms).Include(h=>h.Services).ToListAsync();
            return View(hotels);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHotelVM hotelVM)
        {
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            if (!ModelState.IsValid) return View();
            bool result = _dbContext.Hotels.Any(c => c.Name.Trim().ToLower() == hotelVM.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda Hotel artiq movcuddur");
                return View();
            }
            Hotel hotel = new Hotel
            {
                Name = hotelVM.Name,
                Description = hotelVM.Description,
                Address = hotelVM.Address,
                Email = hotelVM.Email,
                MapLink = hotelVM.MapLink,
                PhoneNumber = hotelVM.PhoneNumber,
                Type = hotelVM.Type,
                WebSite = hotelVM.WebSite,
                Services = new List<HotelService>()
            };
            if (hotelVM.Logo != null)
            {
                if (!hotelVM.Logo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!hotelVM.Logo.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }
                hotel.Logo = await hotelVM.Logo.CreateFileAsync(_env.WebRootPath, "assets/assets/images");
            }
            if (hotelVM.Rating>0 && hotelVM.Rating < 6)
            {
                hotel.Rating = hotelVM.Rating;
            }
            else
            {
                ModelState.AddModelError("Rating","1 ile 5 araliginda qiymet qaxil edin");
                return View();
            }
            foreach (Guid serviceId in hotelVM.ServiceIds)
            {
                if (!await _dbContext.Services.AnyAsync(t => t.Id == serviceId))
                {
                    ModelState.AddModelError("ServiceId", $"{serviceId} id-li service movcud deyil");
                    return View();
                }
                HotelService hotelService = new HotelService
                {
                    ServiceId = serviceId,
                    Hotel = hotel
                };
                hotel.Services.Add(hotelService);
            }
            await _dbContext.Hotels.AddAsync(hotel);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Services = await _dbContext.Services.ToListAsync();

            if (id == null) throw new BadRequestException();
            Hotel existed = await _dbContext.Hotels.Where(h => h.Id == id).Include(h=>h.Rooms).Include(h => h.Services).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            UpdateHotelVM hotelVM = new UpdateHotelVM
            {
                Rating = existed.Rating,
                Name = existed.Name,
                LogoUrl = existed.Logo,
                Description = existed.Description,
                Address = existed.Address,
                Email = existed.Email,
                MapLink = existed.MapLink,
                PhoneNumber = existed.PhoneNumber,
                Type = existed.Type,
                WebSite = existed.WebSite,
                ServiceIds = existed.Services.Select(hs => hs.ServiceId).ToList()
            };
            return View(hotelVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateHotelVM HotelVM)
        {
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            if (id == null) throw new BadRequestException();
            Hotel existed = await _dbContext.Hotels.Where(h => h.Id == id).Include(h => h.Rooms).Include(h => h.Services).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            if (!ModelState.IsValid) return View();
            if (HotelVM.Name != null && !(await _dbContext.Hotels.AnyAsync(c => c.Name.Trim().ToLower() == HotelVM.Name.Trim().ToLower()))) existed.Name = HotelVM.Name;
            if (HotelVM.Type != null && existed.Type.Trim().ToLower() != HotelVM.Type.Trim().ToLower()) existed.Type = HotelVM.Type;
            if (HotelVM.Email != null && !(await _dbContext.Hotels.AnyAsync(c => c.Email.Trim().ToLower() == HotelVM.Email.Trim().ToLower()))) existed.Email = HotelVM.Email;
            if (HotelVM.PhoneNumber != null && !(await _dbContext.Hotels.AnyAsync(c => c.PhoneNumber.Trim().ToLower() == HotelVM.PhoneNumber.Trim().ToLower()))) existed.PhoneNumber = HotelVM.PhoneNumber;
            if (HotelVM.MapLink != null && !(await _dbContext.Hotels.AnyAsync(c => c.MapLink == HotelVM.MapLink))) existed.MapLink = HotelVM.MapLink;
            if (HotelVM.Description != null && existed.Description.Trim().ToLower() != HotelVM.Description.Trim().ToLower()) existed.Description = HotelVM.Description;
            if (HotelVM.Rating > 0 && HotelVM.Rating < 6) existed.Rating = HotelVM.Rating;
            if(HotelVM.Logo != null)
            {
                if (!HotelVM.Logo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!HotelVM.Logo.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }
                existed.Logo.DeleteFile(_env.WebRootPath, "assets/assets/images");
                existed.Logo = await HotelVM.Logo.CreateFileAsync(_env.WebRootPath, "assets/assets/images");
            }
            if (HotelVM.ServiceIds != null && HotelVM.ServiceIds.Count > 0)
            {
                List<Guid> createList = HotelVM.ServiceIds.Where(t => !existed.Services.Any(pt => pt.ServiceId == t)).ToList();
                foreach (Guid serviceId in createList)
                {
                    bool tagResult = await _dbContext.Services.AnyAsync(pt => pt.Id == serviceId);
                    if (tagResult)
                    {

                        HotelService hotelService = new HotelService
                        {
                            HotelId = existed.Id,
                            ServiceId = serviceId
                        };
                        existed.Services.Add(hotelService);
                    }
                }
                List<HotelService> removeList = existed.Services.Where(pt => !HotelVM.ServiceIds.Contains(pt.ServiceId)).ToList();
                _dbContext.HotelServices.RemoveRange(removeList);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) throw new BadRequestException();
            Hotel existed = await _dbContext.Hotels.Where(h => h.Id == id).Include(h=>h.Spa).ThenInclude(s => s.Images).Include(h=>h.Restaurant).ThenInclude(r=>r.Images).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            Spa spa = await _dbContext.Spas.Where(h => h.HotelId == existed.Id).FirstOrDefaultAsync();
            if (spa != null)
            {
                if (spa.Images.Count > 0)
                {
                    foreach (var item in spa.Images)
                    {
                        item.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/spa");
                        _dbContext.SpaImages.Remove(item);
                    }
                }
                _dbContext.Spas.Remove(spa);
            }
            Restaurant restaurant = await _dbContext.Restaurants.Where(h => h.HotelId == existed.Id).FirstOrDefaultAsync();
            if (restaurant != null)
            {
                if (restaurant.Images.Count > 0)
                {
                    foreach (var item in restaurant.Images)
                    {
                        item.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/restaurant");
                        _dbContext.RestaurantImages.Remove(item);
                    }
                }
                _dbContext.Restaurants.Remove(restaurant);
            }
            List<Gallery> galleries = await _dbContext.Galleries.Where(h => h.HotelId == existed.Id).ToListAsync();
            if (galleries != null )
            {
                if (galleries.Count > 0)
                {
                    foreach (var item in galleries)
                    {
                        item.Url.DeleteFile(_env.WebRootPath, @"assets/assets/images/restaurant");
                        _dbContext.Galleries.Remove(item);
                    }
                }
            }
            if (existed.Logo != null)
            {
                existed.Logo.DeleteFile(_env.WebRootPath, @"assets/assets/images/");
            }
            _dbContext.Hotels.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

