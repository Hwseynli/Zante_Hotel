using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zante_Hotel.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(int take = 2, int page = 1)
        {
            ViewBag.TotalPage = (int)Math.Ceiling((double)_dbContext.Blogs.Count() / take);
            ViewBag.CurrentPage = page;
            ICollection<Comment> comments = await _dbContext.Comments.Include(c => c.Replies).ToListAsync();
            ICollection<Blog> blogs = await _dbContext.Blogs.Where(b=>b.CreateOn<DateTime.Now).Include(b => b.Author).Include(b => b.Comments).Include(b => b.Tags).Skip((page - 1) * take).Take(take).ToListAsync();
            BlogVM BlogVM = new BlogVM
            {
                Blogs = blogs,
                Comments = comments
            };
            return View(BlogVM);
        }
        public async Task<IActionResult> BlogPost(Guid? id)
        {
            Blog blog = await _dbContext.Blogs.Where(b => b.Id == id).Include(b => b.Tags).Include(b => b.Comments).Include(b => b.Author).FirstOrDefaultAsync();

            BlogVM blogVM = new BlogVM
            {
                Blog = blog
            };
            return View(blogVM);
        }
    }
}

