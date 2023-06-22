using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(Guid? id)
        {
            ICollection<Comment> comments = await _dbContext.Comments.Include(c => c.Replies).ToListAsync();
            Blog blog = await _dbContext.Blogs.Where(b => b.Id == id).Include(b=>b.Tags).FirstOrDefaultAsync();
            BlogVM BlogVM = new BlogVM
            {
                Blog = blog,
                Comments = comments
            };
            return View(BlogVM);
        }
    }
}

