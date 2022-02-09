using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product = FirstFiorellaMVC.Models.Product;

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

        public async Task<IActionResult> Basket()
        {
            var basket = Request.Cookies["Basket"];

            if (string.IsNullOrEmpty(basket))
            {
                return Content("Empthy");
            }

            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var newBaskets = new List<BasketViewModel>();
            foreach (var item in basketViewModels)
            {
                var product = await _appDbContext.Products.FindAsync(item.Id);
                if (product == null)
                    continue;

                newBaskets.Add(new BasketViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Dimension = product.Dimension,
                    SKUCode = product.SKUCode,
                    Weight = product.Weight,
                    Image = item.Image,
                    Count = item.Count,
                    CampaignId = product.CampaignId,
                    CategoryId = product.CategoryId,
                });
            }

            basket = JsonConvert.SerializeObject(newBaskets);

            Response.Cookies.Append("Basket", basket, new CookieOptions { Expires = System.DateTimeOffset.Now.AddDays(1) });
            return View(newBaskets);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if(id == null)
                return BadRequest();

            var product = await _appDbContext.Products.Include(x=>x.Images).FirstOrDefaultAsync(x=>x.Id == id);
            var image = await _appDbContext.ProductImages.FirstOrDefaultAsync(x => x.ProductId == id && x.IsMain == true);
            
            if (product == null)
                return NotFound();

            List<BasketViewModel> basketViewModels;
            var CookieBasket = Request.Cookies["Basket"];
            if(string.IsNullOrEmpty(CookieBasket))
            {
                basketViewModels = new List<BasketViewModel>();
            }
            else
            {
                basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(CookieBasket);
            }

            var existBasketViewModel = basketViewModels.FirstOrDefault(x => x.Id == id);
            if(existBasketViewModel == null)
            {
                basketViewModels.Add(new BasketViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Dimension = product.Dimension,
                    SKUCode = product.SKUCode,
                    Weight = product.Weight,
                    Image = image.Name,
                    CampaignId = product.CampaignId,
                    CategoryId = product.CategoryId,
                });
            }
            else
            {
                existBasketViewModel.Count++;
            }

            var Basket = JsonConvert.SerializeObject(basketViewModels);

            Response.Cookies.Append("Basket", Basket, new CookieOptions { Expires = System.DateTimeOffset.Now.AddDays(1)});

            return RedirectToAction(nameof(Index));
        }
    }
}
