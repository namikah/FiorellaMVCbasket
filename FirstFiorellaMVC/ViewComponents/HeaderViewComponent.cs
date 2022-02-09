using FirstFiorellaMVC.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var bios = await _dbContext.Bios.SingleOrDefaultAsync();

            return View(bios);
        }
    }
}
