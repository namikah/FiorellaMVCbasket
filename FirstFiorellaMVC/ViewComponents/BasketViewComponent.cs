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
    public class BasketViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public BasketViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket = Request.Cookies["Basket"];

            var selectedProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

            return View(selectedProducts);
        }
    }
}
