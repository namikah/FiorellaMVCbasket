using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View(new HomeViewModel()
            {
                Products = _appDbContext.Products.Include(x=>x.Images).Take(8).ToList(),
                Categories = _appDbContext.Categories.ToList(),
                Slider = _appDbContext.Sliders.SingleOrDefault(),
                SliderImages = _appDbContext.SliderImages.ToList(),
                AboutImage = _appDbContext.AboutImages.SingleOrDefault(),
                AboutContext = _appDbContext.AboutContexts.SingleOrDefault(),
                AboutUnstyledLists = _appDbContext.AboutUnstyledLists.ToList(),
                Experts = _appDbContext.Experts.Include(x => x.Position).ToList(),
                ExpertContext = _appDbContext.ExpertContexts.SingleOrDefault(),
                Positions = _appDbContext.Positions.ToList(),
                Subcribe = _appDbContext.Subcribes.SingleOrDefault(),
                Blogs = _appDbContext.Blogs.ToList(),
                BlogContext = _appDbContext.BlogContexts.SingleOrDefault(),
                Authors = _appDbContext.Authors.ToList(),
                Instagrams = _appDbContext.Instagrams.ToList(),
                Socials = _appDbContext.Socials.ToList(),
                Campaigns = _appDbContext.Campaigns.ToList(),
            });
        }

        public async Task<IActionResult> Search(string searchedProduct)
        {

            if (string.IsNullOrEmpty(searchedProduct))
            {
                return NoContent();
            }

            var products = await _appDbContext.Products.Include(x=>x.Images)
                .Where(x => x.Name.ToLower().Contains(searchedProduct.ToLower()))
                .ToListAsync();

            return PartialView("_SearchProductPartial", products);
        }
    }
}
