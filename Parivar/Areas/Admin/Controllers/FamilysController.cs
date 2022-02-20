using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Exceptions;
using Parivar.Repository.Interface;
using Parivar.Utility;
using Parivar.Utility.Common;
using Parivar.Utility.Extension;
using Parivar.ViewModel;

namespace Parivar.Areas.Admin.Controllers
{
    [Authorize, Area("Admin")]
    public class FamilysController : BaseController<FamilysController>
    {
        private readonly IFamilyMemberDetailsService _familyMemberDetails;
        private readonly IFamilyUserService _familyUser;
        public FamilysController(IFamilyMemberDetailsService familyMemberDetails, IFamilyUserService familyUser)
        {
            _familyMemberDetails = familyMemberDetails;
            _familyUser = familyUser;
        }

        #region Family
        public IActionResult Index()
        {
            BindDropdownList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFamilyList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0));

                var allList = await _familyMemberDetails.GetFamilyList(parameters.Parameters.ToArray());
                parameters.SearchRecord = parameters.Parameters.Last();

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
                ErrorLog.AddErrorLog(ex, "GetFamilyList");
                return JsonResponse.GenerateJsonResult(0, "Something went wrong.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFamilyMemberList(long memberId, JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0));
                parameters.Parameters.Insert(0, new SqlParameter("@MainMemberId", SqlDbType.BigInt) { Value = memberId });

                var allList = await _familyMemberDetails.GetFamilyMemberList(parameters.Parameters.ToArray());
                parameters.SearchRecord = parameters.Parameters.Last();

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
                ErrorLog.AddErrorLog(ex, "GetFamilyMemberList");
                return JsonResponse.GenerateJsonResult(0, "Something went wrong.");
            }
        }

        [HttpGet]
        [Route("/Admin/Family/{id}")]
        public IActionResult ViewFamily(long id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageFamilyStatus(long id)
        {
            using (TransactionScope txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _familyUser.GetSingle(x => x.Id == id);
                    result.IsActive = !result.IsActive;
                    _familyUser.Update(result, User.GetUserId());
                    txscope.Complete();
                    return JsonResponse.GenerateJsonResult(1, $@"Family {(result.IsActive ? "activated" : "deactivated")} successfully.", result.Id);
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/ManageFamilyStatus");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        //[HttpPost]
        //public IActionResult RemoveFamily(long id)
        //{
        //    using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        try
        //        {
        //            var result = _familyUser.GetSingle(x => x.Id == id);
        //            _familyUser.Delete(result);
        //            _familyUser.Save();
        //            txscope.Complete();
        //            return JsonResponse.GenerateJsonResult(1, @"Family deleted successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            txscope.Dispose();
        //            ErrorLog.AddErrorLog(ex, "Post/RemoveCategories");
        //            return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
        //        }
        //    }
        //}
        #endregion

        #region Common     
        private void BindDropdownList()
        {
        }
        #endregion
    }
}