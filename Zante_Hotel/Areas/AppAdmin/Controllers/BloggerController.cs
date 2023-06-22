using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class BloggerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public BloggerController(AppDbContext dbContext, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _env = env;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            List<Blog> blogs = await _dbContext.Blogs.Where(b=>b.Author.UserName ==User.Identity.Name).Include(p => p.Tags).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            if (!ModelState.IsValid) return View();
            if (!blogVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                return View();
            }
            if (!blogVM.Photo.CheckFileSize(2000))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 200 kb-den boyuk olmamalidir");
                return View();
            }
            string username = User.Identity.Name;
            Blog blog = new Blog
            {
                Name = blogVM.Name,
                Description = blogVM.Description,
                Author = await _userManager.FindByNameAsync(username),
                Comments = blogVM.Comments,
                Tags=blogVM.Tags,
                
            };
            blog.ImgUrl = await blogVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/assets/images/blog");
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            if (id == null) return BadRequest(ModelState);
            Blog blog = await _dbContext.Blogs.Where(b=>b.Id==id).FirstOrDefaultAsync();
            if (blog == null) return NotFound();
            UpdateBlogVM updateBlog = new UpdateBlogVM
            {
                Name = blog.Name,
                Description = blog.Description,
                Comments = blog.Comments,
                Tags = blog.Tags,
                ImgUrl=blog.ImgUrl
            };
            return View(updateBlog);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateBlogVM blogVM)
        {
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            if (id == null) return BadRequest();
            Blog existed = await _dbContext.Blogs.Include(e => e.Tags).FirstOrDefaultAsync(e => e.Id == id);
            if (existed == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            if (blogVM.Photo != null)
            {
                if (!blogVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!blogVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200 kb den cox olmamalidir");
                    return View();
                }
                existed.ImgUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/blog");
                existed.ImgUrl = await blogVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/assets/images/blog");
            }

            if (blogVM.Name != null && blogVM.Name != existed.Name) existed.Name = blogVM.Name;
            if (blogVM.Description != null && blogVM.Description != existed.Description) existed.Description = blogVM.Description;
            if (blogVM.Tags.Count > 0 && blogVM.Tags != null) existed.Tags = blogVM.Tags;
            if (blogVM.Comments != null && blogVM.Comments != null) existed.Comments = blogVM.Comments;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Blog blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();
            blog.ImgUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/blog");
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}