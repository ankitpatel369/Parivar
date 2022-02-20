using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Interface;
using Parivar.Utility;
using Parivar.Utility.Common;

namespace Parivar.Controllers
{
    public class ParivarController : BaseController<ParivarController>
    {
        public ParivarController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Committees()
        {
            return View();
        }
        public IActionResult HighEducatedPeoples()
        {
            return View();
        }
        public IActionResult NoticeBoard()
        {
            return View();
        }
        public IActionResult DevPlaces()
        {
            return View();
        }
        public IActionResult Business()
        {
            return View();
        }
        public IActionResult Events()
        {
            return View();
        }

        public IActionResult EventDetails()
        {
            return View();
        }
        
        public IActionResult Gallerys()
        {
            return View();
        }
    }
}
