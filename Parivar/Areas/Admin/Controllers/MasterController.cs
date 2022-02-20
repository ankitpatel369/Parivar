using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Exceptions;
using Parivar.Repository.Interface;
using Parivar.Utility;
using Parivar.Utility.Common;
using Parivar.ViewModel;

namespace Parivar.Areas.Admin.Controllers
{
    [Authorize, Area("Admin")]
    public class MasterController : BaseController<MasterController>
    {
        private readonly IContactUsService _contactUs;

        public MasterController(IContactUsService contactUs)
        {
            _contactUs = contactUs;
        }

        [Route("/Admin/Master/ContactUs")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetContactUsList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _contactUs.GetContactUsList(parameters.ToArray());
                var total = allList.FirstOrDefault()?.TotalRecords ?? 0;
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = total,
                    iTotalDisplayRecords = total,
                    aaData = allList
                });
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex, "GetContactUsList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }

        [HttpGet]
        public IActionResult ContactUsDetails(long id)
        {
            var result = Mapper.Map<ContactUsModel>(_contactUs.GetSingle(x => x.Id == id));
            return PartialView(@"Partial/_ContactUsDetails", result);
        }

        [HttpPost]
        public IActionResult ReadContactUs(long id)
        {
            using (TransactionScope txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _contactUs.GetSingle(x => x.Id == id);
                    result.IsRead = !result.IsRead;
                    _contactUs.Update(result, User.GetUserId());
                    txscope.Complete();
                    return JsonResponse.GenerateJsonResult(1, null, result.Id);
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/ReadContactUs");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }
    }
}
