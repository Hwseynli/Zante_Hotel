using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;

        public RoomController(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Room> rooms = await _dbContext.Rooms.Include(r=>r.Category).Include(r => r.View).Include(r => r.Services).ThenInclude(rs=>rs.Service).Include(r => r.ReservationsDate).Include(r=>r.Images.Where(ri=>ri.IsPrimary)).ToListAsync();
            return View(rooms);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();
            ViewBag.Views = await _dbContext.Views.ToListAsync();
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomVM roomVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();
            ViewBag.Views = await _dbContext.Views.ToListAsync();
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            if (!ModelState.IsValid) return View();
            bool resultcat = await _dbContext.Categories.AnyAsync(c => c.Id == roomVM.CategoryId);
            if (!resultcat)
            {
                ModelState.AddModelError("CategoryId", "Bu id-li category movcud deyil");
                return View();
            }
            bool resultview = await _dbContext.Views.AnyAsync(c => c.Id == roomVM.ViewId);
            if (!resultview)
            {
                ModelState.AddModelError("ViewId", "Bu id-li view movcud deyil");
                return View();
            }
            if(await _dbContext.Rooms.AnyAsync(c => c.Number == roomVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli otaq artiq movcuddur");
                return View();
            }
            if (!await _dbContext.Hotels.AnyAsync(h => h.Id == roomVM.HotelId))
            {
                ModelState.AddModelError("HotelId", "Bu id-li view movcud deyil");
                return View();
            }
            Room room = new Room
            {
                Number=roomVM.Name,
                HotelId=roomVM.HotelId,
                Description = roomVM.Description,
                Price=roomVM.Price,
                CategoryId=roomVM.CategoryId,
                ViewId=roomVM.ViewId,
                Services=new List<RoomServices>(),
                Images=new List<RoomImage>()
            };
          
            if (roomVM.NumberOfPeople < 0 && roomVM.NumberOfPeople > 10)
            {
                ModelState.AddModelError("NumberOfPeople", "Sayi duzgun daxil edin");
                return View();
            }
            room.NumberOfPeople = roomVM.NumberOfPeople;
            foreach (Guid serviceId in roomVM.ServiceIds)
            {
                bool serviceResult = await _dbContext.Services.AnyAsync(t => t.Id == serviceId);
                if (!serviceResult)
                { 
                    ModelState.AddModelError("ServiceId", $"{serviceId} id-li service movcud deyil");
                    return View();
                }
                RoomServices roomService = new RoomServices
                {
                    ServiceId = serviceId,
                    Room = room
                };
                room.Services.Add(roomService);
            }
            if (roomVM.MainPhoto != null)
            {
                if (!roomVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!roomVM.MainPhoto.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }

                room.Images.Add(new RoomImage
                {
                    ImageUrl = await roomVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/rooms"),
                    IsPrimary = true,
                    Room = room
                });
            }
            foreach (IFormFile photo in roomVM.Photos)
            {
                if (photo != null)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File tipi uygun deyil");
                        return View();
                    }
                    if (!photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photos", "File olcusu uygun deyil");
                        return View();
                    }
                    RoomImage addImage = new RoomImage
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/rooms"),
                        Room = room
                    };
                    room.Images.Add(addImage);
                }
            }
            await _dbContext.Rooms.AddAsync(room);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();
            ViewBag.Views = await _dbContext.Views.ToListAsync();
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            if (id is null) return BadRequest();
            Room existed = await _dbContext.Rooms.Where(r=>r.Id==id).Include(r=>r.Images).Include(r=>r.Category).Include(r => r.Services).Include(r => r.View).FirstOrDefaultAsync();
            if (existed == null) return NotFound();
            UpdateRoomVM roomVM = new UpdateRoomVM
            {
                HotelId=existed.HotelId,
                Name = existed.Number,
                Price = existed.Price,
                NumberOfPeople = existed.NumberOfPeople,
                Description = existed.Description,
                CategoryId = existed.CategoryId,
                ViewId = existed.ViewId,
                ServiceIds = existed.Services.Select(rs => rs.ServiceId).ToList(),
                ReservationsDate = existed.ReservationsDate
            };
            roomVM = MapImages(roomVM, existed);
            return View(roomVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id,UpdateRoomVM roomVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();
            ViewBag.Views = await _dbContext.Views.ToListAsync();
            ViewBag.Services = await _dbContext.Services.ToListAsync();
            if (id is null) return BadRequest();
            Room existed = await _dbContext.Rooms.Where(r => r.Id == id).Include(r => r.Images).Include(r => r.Category).Include(r => r.Services).Include(r => r.View).Include(r => r.ReservationsDate).FirstOrDefaultAsync();
            if (existed == null) return NotFound();
            roomVM = MapImages(roomVM, existed);
            if (!ModelState.IsValid) return View(roomVM);
            if (await _dbContext.Hotels.AnyAsync(h=>h.Id==roomVM.HotelId) && roomVM.HotelId != existed.HotelId) existed.HotelId = roomVM.HotelId;
            if (await _dbContext.Views.AnyAsync(h => h.Id == roomVM.ViewId) && roomVM.ViewId != existed.ViewId) existed.ViewId = roomVM.ViewId;
            if (await _dbContext.Categories.AnyAsync(h => h.Id == roomVM.CategoryId) && roomVM.CategoryId != existed.CategoryId) existed.CategoryId = roomVM.CategoryId;
            if (roomVM.Name != null && roomVM.Name != existed.Number) existed.Number = roomVM.Name;
            if (roomVM.Description != null && roomVM.Description != existed.Description) existed.Description = roomVM.Description;
            if (roomVM.Price > 0 && roomVM.Price != existed.Price) existed.Price = roomVM.Price;
            if (roomVM.NumberOfPeople > 0 && roomVM.NumberOfPeople < 10 && roomVM.NumberOfPeople != existed.NumberOfPeople) existed.NumberOfPeople = roomVM.NumberOfPeople;
            if (roomVM.ServiceIds != null && roomVM.ServiceIds.Count>0)
            {
                List<Guid> createList = roomVM.ServiceIds.Where(t => !existed.Services.Any(pt => pt.ServiceId == t)).ToList();
                foreach (Guid serviceId in createList)
                {
                    bool tagResult = await _dbContext.Services.AnyAsync(pt => pt.Id == serviceId);
                    if (tagResult)
                    {

                        RoomServices roomService = new RoomServices
                        {
                            RoomId = existed.Id,
                            ServiceId = serviceId
                        };
                        existed.Services.Add(roomService);
                    }
                }
                List<RoomServices> removeList = existed.Services.Where(pt => !roomVM.ServiceIds.Contains(pt.ServiceId)).ToList();
                _dbContext.RoomServices.RemoveRange(removeList);
            }
            if (roomVM.MainPhoto != null)
            {
                if (!roomVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin novu uygun deyil");
                    return View(roomVM);
                }
                if (!roomVM.MainPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin olcusu uygun deyil");
                    return View(roomVM);
                }
                var mainImage = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/rooms");
                existed.Images.Remove(mainImage);
                RoomImage roomImage = new RoomImage
                {
                    RoomId = existed.Id,
                    ImageUrl = await roomVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/rooms"),
                    IsPrimary = true
                };
                existed.Images.Add(roomImage);
            }
            List<RoomImage> removeImageList = existed.Images.Where(pi => !roomVM.ImagesIds.Contains(pi.Id) && pi.IsPrimary == false).ToList();
            foreach (RoomImage rImage in removeImageList)
            {
                rImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/rooms");
                existed.Images.Remove(rImage);
            }
            if (roomVM.Photos != null && roomVM.Photos.Count > 0)
            {
                foreach (var photo in roomVM.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File tipi uygun deyil");
                        return View();
                    }
                    if (!photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photos", "File olcusu uygun deyil");
                        return View();
                    }
                    existed.Images.Add(new RoomImage
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/rooms"),
                        IsPrimary = false,
                        RoomId = existed.Id
                    });
                }
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) return BadRequest();
            Room existed = await _dbContext.Rooms.Include(r=>r.Images).FirstOrDefaultAsync(r=>r.Id==id);
            if (existed == null) return NotFound();
            if (existed.Images.Count>0)
            {
                foreach (var item in existed.Images)
                {
                    item.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/rooms");
                    _dbContext.RoomImages.Remove(item);
                }
            }
            _dbContext.Rooms.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public UpdateRoomVM MapImages(UpdateRoomVM roomVM, Room room)
        {
            roomVM.RoomImageVMs = new List<RoomImageVM>();
            foreach (RoomImage image in room.Images)
            {
                RoomImageVM imageVM = new RoomImageVM
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    IsPrimary = image.IsPrimary
                };
                roomVM.RoomImageVMs.Add(imageVM);
            }
            return roomVM;
        }
    }
}