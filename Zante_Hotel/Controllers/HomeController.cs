using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _http;

    public HomeController(AppDbContext dbContext, UserManager<AppUser> userManager, IHttpContextAccessor http)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _http = http;
    }
    public async Task<IActionResult> Index()
    {
        Hotel hotel = await _dbContext.Hotels.Include(h => h.Spa).ThenInclude(s => s.Images.Where(i => i.IsPrimary == true)).Include(h => h.Restaurant).ThenInclude(s => s.Images.Where(i=>i.IsPrimary==true)).Include(h=>h.Services.Where(s=>s.Service.Id== Guid.Parse("665a630b-4176-443b-4992-08db79a850c0"))).ThenInclude(s=>s.Service).Include(h=>h.Rooms.Where(c=>c.CategoryId== Guid.Parse("98d2f19f-3e82-44d6-ea49-08db79bd8445"))).ThenInclude(r => r.Images.Where(i=>i.IsPrimary)).FirstOrDefaultAsync();
        ICollection<Gallery> galleries = await _dbContext.Galleries.Take(12).ToListAsync();
        Slider slider = await _dbContext.Sliders.Where(s => s.VideoUrl != null).FirstOrDefaultAsync();
        ICollection<Room> rooms = await _dbContext.Rooms.Take(3).Include(r => r.Images.Where(ri => ri.IsPrimary)).Include(r => r.Category).Include(r => r.View).Include(r => r.Services).ToListAsync();
        HomeVM homeVM = new HomeVM
        {
            Hotel=hotel,
            Rooms = rooms,
            Slider = slider,
            Galleries = galleries
        };
        return View(homeVM);
    }
    public IActionResult ContactUs()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ContactUs(Message message)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("notfound", "error");
            }
            else
            {

                Message newMessage = new Message
                {
                    Name = message.Name,
                    Surname = message.Surname,
                    PhoneNumber = message.PhoneNumber,
                    Email = message.Email,
                    Subject = message.Subject,
                    Body = message.Body
                };
                if (await _dbContext.Messages.AnyAsync(m=>m.Email==user.Email))
                {
                    Message exMesage = await _dbContext.Messages.Where(m => m.Email == newMessage.Email).FirstOrDefaultAsync();
                    _dbContext.Messages.Remove(exMesage);
                }
                _dbContext.Messages.Add(newMessage);
                await _dbContext.SaveChangesAsync();
                ViewBag.Message = "Your message has been sent successfully";
                return RedirectToAction();
            }
        }
        ModelState.AddModelError(string.Empty, "Please Firstly Sign in or Sign Up");
        return View();
    }
    public async Task<IActionResult> Spa()
    {
        HomeVM homeVM = new HomeVM
        {
            Spa = await _dbContext.Spas.Include(s=>s.Hotel).Include(s=>s.Images).FirstOrDefaultAsync()
        };
        return View(homeVM);
    }
    public async Task<IActionResult> Restaurant()
    {
        HomeVM homeVM = new HomeVM
        {
            Restaurant = await _dbContext.Restaurants.Include(r => r.Hotel).Include(r=>r.RestFoods).ThenInclude(rf=>rf.Food).Include(s => s.Images).FirstOrDefaultAsync()
        };
        return View(homeVM);
    }
}

