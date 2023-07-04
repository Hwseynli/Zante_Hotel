using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zante_Hotel.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles =$"Admin, Blogger")]
    public class BloggerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _http;

        public BloggerController(AppDbContext dbContext, IWebHostEnvironment env, UserManager<AppUser> userManager, IHttpContextAccessor http)
        {
            _dbContext = dbContext;
            _env = env;
            _userManager = userManager;
            _http = http;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _dbContext.Blogs
                //.Where(b => b.Author.UserName == _http.HttpContext.User.Identity.Name)
                .Include(p => p.Tags)
                .ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.ToListAsync();
            if (!ModelState.IsValid) return View();
            if (!blogVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                return View();
            }
            if (!blogVM.Photo.CheckFileSize(20000))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 200 kb-den boyuk olmamalidir");
                return View();
            }
            if(_http.HttpContext.User.Identity.Name == null)
            {
                ModelState.AddModelError(string.Empty, "Login olunmalisiniz");
                return View();
            }
            string username = _http.HttpContext.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception("nese o deyile");
            }
            if (_dbContext.Blogs.Any(b=>b.AuthorId==user.Id && b.Name==blogVM.Name))
                {
                ModelState.AddModelError("Name", "Basqqa ad qoymalisiniz bloga");
                return View();
                }
            if (await _dbContext.Blogs.AnyAsync(b => b.AuthorId == user.Id && b.SubTitle == blogVM.SubTitle))
            {
                ModelState.AddModelError("SubTitle", "Basqqa basliq qoymalisiniz bloga");
                return View();
            }
            if (!await _dbContext.Hotels.AnyAsync(h => h.Id == blogVM.HotelId))
            {
                ModelState.AddModelError("HotelId", "Bu id li hotel tapilmadi");
                return View();
            }
            Blog blog = new Blog
            {
                HotelId=blogVM.HotelId,
                Name = blogVM.Name,
                CreateOn=DateTime.Now,
                SubTitle=blogVM.SubTitle,
                Description = blogVM.Description,
                Author = user,
                Comments = blogVM.Comments,
                Tags = new List<BlogTag>(),
            };
            foreach (Guid tagId in blogVM.TagIds)
            {
                bool tagResult = await _dbContext.Tags.AnyAsync(t => t.Id == tagId);
                if (!tagResult)
                {
                    ModelState.AddModelError("TagIds", $"{tagId} id-li Tag movcud deyil");
                    return View();
                }
                BlogTag blogTag = new BlogTag
                {
                    TagId = tagId,
                    Blog = blog
                };
                blog.Tags.Add(blogTag);
            }

            blog.ImgUrl = await blogVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/assets/images/blog");
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            if (id == null) return BadRequest();
            Blog blog = await _dbContext.Blogs.Where(b => b.Id == id).Include(b=>b.Tags).FirstOrDefaultAsync();
            if (blog == null) return NotFound();
            AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            if (user.Id != blog.AuthorId)
            {
                return View();
            }
            UpdateBlogVM updateBlog = new UpdateBlogVM
            {
                HotelId=blog.HotelId,
                Name = blog.Name,
                SubTitle=blog.SubTitle,
                Description = blog.Description,
                TagIds = blog.Tags.Select(bt=>bt.TagId).ToList(),
                ImgUrl = blog.ImgUrl
            };
            return View(updateBlog);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateBlogVM blogVM)
        {
            ViewBag.Hotels = await _dbContext.Hotels.ToListAsync();
            ViewBag.Tags = await _dbContext.Tags.Include(c => c.Blogs).ToListAsync();
            if (id == null) return BadRequest();
            Blog existed = await _dbContext.Blogs.Where(b => b.Id == id).Include(b => b.Tags).FirstOrDefaultAsync();
            if (existed == null) return NotFound();
            AppUser user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            if (user.Id!=existed.AuthorId)
            {
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
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
            if (blogVM.HotelId != existed.HotelId && await _dbContext.Hotels.AnyAsync(h => h.Id == blogVM.HotelId)) existed.HotelId = blogVM.HotelId;
            if (blogVM.Name != null && blogVM.Name != existed.Name && !( _dbContext.Blogs.Any(b => b.AuthorId == user.Id && b.Name == blogVM.Name))) existed.Name = blogVM.Name;
            if (blogVM.SubTitle != null && blogVM.SubTitle != existed.SubTitle && !( _dbContext.Blogs.Any(b => b.AuthorId == user.Id && b.Name == blogVM.Name))) existed.SubTitle = blogVM.SubTitle;
            if (blogVM.Description != null && blogVM.Description != existed.Description) existed.Description = blogVM.Description;
            if (blogVM.TagIds is null)
            {
                ModelState.AddModelError("TagIds", "En azi 1 tag secin");
                return View(blogVM);
            }
             List<Guid> createList = blogVM.TagIds.Where(t => !existed.Tags.Any(pt => pt.TagId == t)).ToList();
            foreach (Guid tagId in createList)
            {
                bool tagResult = await _dbContext.Tags.AnyAsync(pt=>pt.Id==tagId);
                if (!tagResult)
                {
                    ViewBag.Tags = await _dbContext.Tags.ToListAsync();
                    ModelState.AddModelError("TagIds", "Bele tag movcud deyil");
                    return View(blogVM);
                }
                BlogTag productTag = new BlogTag
                {
                    BlogId = existed.Id,
                    TagId = tagId
                };
                existed.Tags.Add(productTag);
            }

            List<BlogTag> removeList = existed.Tags.Where(pt => !blogVM.TagIds.Contains(pt.TagId)).ToList();
            _dbContext.BlogTags.RemoveRange(removeList);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Blog blog = await _dbContext.Blogs.Where(b => b.Id == id).Include(b=>b.Author).Include(b=>b.Tags).FirstOrDefaultAsync();
            if (blog == null) return NotFound();
            if (_http.HttpContext.User.Identity.Name == null)
            {
                ModelState.AddModelError(string.Empty, "Login olunmalisiniz");
                return View();
            }
            string username = _http.HttpContext.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception("nese o deyile");
            }
            if (_dbContext.Blogs.Any(b => b.AuthorId != user.Id))
            {
                ModelState.AddModelError(string.Empty, "Siz bu blogu yaratmamisiz ki sile de bilesiniz...");
                return View();
            }
            if (blog.ImgUrl!=null) { 
                blog.ImgUrl.DeleteFile(_env.WebRootPath, @"assets/assets/images/blog");
            }
            ICollection<BlogTag> removeList = blog.Tags.ToList();
            _dbContext.BlogTags.RemoveRange(removeList);
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}