using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zante_Hotel.Utilities.Exceptions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class RestaurantController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _dbContext;
        public RestaurantController(IWebHostEnvironment env, AppDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ICollection<Restaurant> restaurants = await _dbContext.Restaurants.Include(s => s.Images).Include(s => s.Hotel).ToListAsync();
            return View(restaurants);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Foods = await _dbContext.Foods.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantVM restaurantVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Foods = await _dbContext.Foods.ToListAsync();
            if (!ModelState.IsValid) return View();
            bool resultcat = await _dbContext.Hotels.AnyAsync(c => c.Id == restaurantVM.HotelId);
            if (!resultcat)
            {
                ModelState.AddModelError("HotelId", "Bu id-li hotel movcud deyil");
                return View();
            }
            if (await _dbContext.Restaurants.AnyAsync(c => c.Name == restaurantVM.Name))
            {
                ModelState.AddModelError("Name", "Bu adli restaurant artiq movcuddur");
                return View();
            }
            Restaurant restaurant = new Restaurant
            {
                SubTitle = restaurantVM.SubTitle,
                Name = restaurantVM.Name,
                HotelId = restaurantVM.HotelId,
                Description = restaurantVM.Description,
                Images = new List<RestaurantImage>(),
                RestFoods=new List<RestaurantFood>()
            };

            if (restaurantVM.MaxPeople > 0) restaurant.MaxPeople = restaurantVM.MaxPeople;
            foreach (Guid foodId in restaurantVM.FoodIds)
            {
                bool foodResult = await _dbContext.Foods.AnyAsync(t => t.Id == foodId);
                if (!foodResult)
                {
                    ModelState.AddModelError("FoodIds", $"{foodId} id-li food movcud deyil");
                    return View();
                }
                RestaurantFood restaurantFood = new RestaurantFood
                {
                    Restaurant = restaurant,
                    FoodId = foodId
                };
                restaurant.RestFoods.Add(restaurantFood);
            }
            if (restaurantVM.MainPhoto != null)
            {
                if (!restaurantVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!restaurantVM.MainPhoto.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }

                restaurant.Images.Add(new RestaurantImage
                {
                    ImageUrl = await restaurantVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/restaurant"),
                    IsPrimary = true,
                    Restourant = restaurant
                });
            }
            if (restaurantVM.Photos != null && restaurantVM.Photos.Count > 0)
            {
                foreach (var photo in restaurantVM.Photos)
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
                        RestaurantImage addImage = new RestaurantImage
                        {
                            ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/restaurant"),
                            Restourant = restaurant,
                            IsPrimary = false
                        };
                        restaurant.Images.Add(addImage);
                    }
                }
            }
            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Foods = await _dbContext.Foods.ToListAsync();
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            if (id is null) throw new BadRequestException();
            Restaurant existed = await _dbContext.Restaurants.Where(r => r.Id == id).Include(r => r.Images).Include(s => s.Hotel).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            UpdateRestaurantVM restaurantVM = new UpdateRestaurantVM
            {
                MaxPeople = existed.MaxPeople,
                HotelId = existed.HotelId,
                Name = existed.Name,
                SubTitle = existed.SubTitle,
                Description = existed.Description,
                FoodIds= existed.RestFoods.Select(rs => rs.FoodId).ToList()
            };
            restaurantVM = MapImages(restaurantVM, existed);
            return View(restaurantVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateRestaurantVM restaurantVM)
        {
            ViewBag.Foods = await _dbContext.Foods.ToListAsync();
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            if (id is null) throw new BadRequestException();
            Restaurant existed = await _dbContext.Restaurants.Where(r => r.Id == id).Include(r => r.Images).Include(s => s.Hotel).FirstOrDefaultAsync();
            if (existed == null) throw new NotFoundException();
            restaurantVM = MapImages(restaurantVM, existed);
            if (!ModelState.IsValid) return View(restaurantVM);
            if (await _dbContext.Hotels.AnyAsync(h => h.Id == restaurantVM.HotelId) && restaurantVM.HotelId != existed.HotelId) existed.HotelId = restaurantVM.HotelId;
            if (restaurantVM.Description != null && restaurantVM.Description != existed.Description) existed.Description = restaurantVM.Description;
            if (restaurantVM.Name != null && restaurantVM.Name != existed.Name) existed.Name = restaurantVM.Name;
            if (restaurantVM.MaxPeople > 0 && restaurantVM.MaxPeople != existed.MaxPeople) existed.MaxPeople = restaurantVM.MaxPeople;
            if (restaurantVM.SubTitle != null && restaurantVM.SubTitle != existed.SubTitle) existed.SubTitle = restaurantVM.SubTitle;
            if (restaurantVM.FoodIds != null && restaurantVM.FoodIds.Count > 0)
            {
                List<Guid> createList = restaurantVM.FoodIds.Where(t => !existed.RestFoods.Any(pt => pt.FoodId == t)).ToList();
                foreach (Guid foodId in createList)
                {
                    bool tagResult = await _dbContext.Foods.AnyAsync(pt => pt.Id == foodId);
                    if (tagResult)
                    {

                        RestaurantFood restaurantFood = new RestaurantFood
                        {
                            RestaurantId = existed.Id,
                            FoodId = foodId
                        };
                        existed.RestFoods.Add(restaurantFood);
                    }
                }
                List<RestaurantFood> removeList = existed.RestFoods.Where(pt => !restaurantVM.FoodIds.Contains(pt.FoodId)).ToList();
                _dbContext.RestaurantFoods.RemoveRange(removeList);
            }
            if (restaurantVM.MainPhoto != null)
            {
                if (!restaurantVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin novu uygun deyil");
                    return View(restaurantVM);
                }
                if (!restaurantVM.MainPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin olcusu uygun deyil");
                    return View(restaurantVM);
                }
                var mainImage = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/restaurant");
                existed.Images.Remove(mainImage);
                RestaurantImage restaurantImage = new RestaurantImage
                {
                    RestaurantId = existed.Id,
                    ImageUrl = await restaurantVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets/assets/images/restaurant"),
                    IsPrimary = true
                };
                existed.Images.Add(restaurantImage);
            }
            List<RestaurantImage> removeImageList = existed.Images.Where(pi => !restaurantVM.ImageIds.Contains(pi.Id) && pi.IsPrimary == false).ToList();
            foreach (RestaurantImage rImage in removeImageList)
            {
                rImage.ImageUrl.DeleteFile(_env.WebRootPath, "assets/assets/images/restaurant");
                existed.Images.Remove(rImage);
            }
            if (restaurantVM.Photos != null && restaurantVM.Photos.Count > 0)
            {
                foreach (var photo in restaurantVM.Photos)
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
                    existed.Images.Add(new RestaurantImage
                    {
                        ImageUrl = await photo.CreateFileAsync(_env.WebRootPath, "assets/assets/images/restaurant"),
                        IsPrimary = false,
                        RestaurantId = existed.Id
                    });
                }
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) throw new BadRequestException();
            Restaurant existed = await _dbContext.Restaurants.Include(r => r.Images).FirstOrDefaultAsync(r => r.Id == id);
            if (existed == null) throw new NotFoundException();
            if (existed.Images.Count > 0)
            {
                foreach (var item in existed.Images)
                {
                    item.ImageUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/restaurant");
                    _dbContext.RestaurantImages.Remove(item);
                }
            }
            ICollection<RestaurantFood> removeList= existed.RestFoods.ToList();
            _dbContext.RestaurantFoods.RemoveRange(removeList);
            _dbContext.Restaurants.Remove(existed);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public UpdateRestaurantVM MapImages(UpdateRestaurantVM restaurantVM, Restaurant restaurant)
        {
            restaurantVM.ImageVMs = new List<RestaurantImageVM>();
            foreach (RestaurantImage image in restaurant.Images)
            {
                RestaurantImageVM imageVM = new RestaurantImageVM
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    IsPrimary = image.IsPrimary,
                };
                restaurantVM.ImageVMs.Add(imageVM);
            }
            return restaurantVM;
        }
    }

}
