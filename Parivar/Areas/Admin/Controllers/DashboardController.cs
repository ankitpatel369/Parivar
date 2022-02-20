using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parivar.Utility;

namespace Parivar.Areas.Admin.Controllers
{
    [Authorize, Area("Admin")]
    public class DashboardController : BaseController<DashboardController>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}