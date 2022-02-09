using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public HeaderViewComponent(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = 0;
            double TotalPrice = 0;
            
            var basket = Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basket))
            {
                var basketList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
                var campaigns = _dbContext.Campaigns.ToList();
                count = basketList.Count;

                foreach (var item in basketList)
                {
                    int discount = 1;
                    if (item.CampaignId != null)
                    {
                        discount = campaigns.Find(x => x.Id == item.CampaignId).Discount;
                        TotalPrice += (item.Price - item.Price * discount/100) * item.Count;
                    }
                    else
                    {
                        TotalPrice += item.Price * item.Count;
                    }
                }
            }
            ViewData["BasketCount"] = count;
            ViewData["BasketTotalPrice"] = TotalPrice;

            var bios = await _dbContext.Bios.SingleOrDefaultAsync();

            return View(bios);
        }
    }
}
