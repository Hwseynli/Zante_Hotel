namespace Zante_Hotel.Services
{
	public class LayoutService
	{
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<AppUser> _manager;

        public LayoutService(AppDbContext dbContext, IHttpContextAccessor http, UserManager<AppUser> manager)
		{
            _dbContext = dbContext;
            _http = http;
            _manager = manager;
        }

        public async Task<AppUser> GetUser()
        {
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _manager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                return user;
            }
            return new AppUser();
        }
    }
}

