using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parivar.Utility;

namespace Parivar.Areas.Member.Controllers
{
    [Authorize, Area("Member")]
    public class DashboardController : BaseController<DashboardController>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}