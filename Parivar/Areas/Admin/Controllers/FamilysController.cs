using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Dto.Enum;
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
        private readonly UserManager<FamilyUser> _userManager;

        public FamilysController(IFamilyMemberDetailsService familyMemberDetails, IFamilyUserService familyUser,
            UserManager<FamilyUser> userManager)
        {
            _familyMemberDetails = familyMemberDetails;
            _familyUser = familyUser;
            _userManager = userManager;
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

        [HttpGet]
        [Route("/Admin/EditFamily/{id}")]
        public async Task<IActionResult> EditFamily(long id)
        {
            ViewBag.IsBreadcrumb = true;
            var mainMember = await _familyUser.GetSingleAsync(x => x.Id == id);
            var familyModel = Mapper.Map<FamilyUser, FamilyModel>(mainMember);
            familyModel.FamilyMemberDetails = Mapper.Map<List<FamilyMemberDetails>, List<FamilyMemberDetailsModel>>(mainMember.FamilyMemberDetails.ToList());

            return View(familyModel);
        }

        [HttpGet]
        [Route("/Admin/MemberInfo/{id}")]
        public IActionResult MemberInfo(long id)
        {
            ViewBag.IsBreadcrumb = true;
            var mainMember = _familyMemberDetails.GetById(id);
            var userProfileInfo = new ProfileViewModel()
            {
                Id = mainMember.Id,
                FullName = mainMember.FullName,
                GenderString = GetGender(mainMember.Gender.HasValue ? mainMember.Gender.Value : 0),
                AdvanceSkills = mainMember.AdvanceKills,
                BloodGroup = mainMember.BloodGroup?.CategoryName ?? "",
                Bussioness = mainMember.Bussioness?.CategoryName ?? "",
                Education = mainMember.Education?.CategoryName ?? "",
                MosalSurname = mainMember.MosalSurname,
                MosalVillage = mainMember.MosalVillage
            };
            return View(userProfileInfo);
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

        [HttpPost]
        public async Task<IActionResult> DeleteFamily(long id)
        {
            using (TransactionScope txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = _familyUser.GetSingle(x => x.Id == id);
                    if (user != null)
                    {
                        var userRole = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRole);
                        _familyMemberDetails.DeleteRange(user.FamilyMemberDetails);
                        _familyUser.Delete(user);
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, $@"Family deleted successfully.");
                    }
                    else
                    {
                        return JsonResponse.GenerateJsonResult(1, $@"Family not exist.");
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/DeleteFamily");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFamilyMember(long id)
        {
            using (TransactionScope txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var familyMember = await _familyMemberDetails.GetSingleAsync(x => x.Id == id);
                    if (familyMember != null)
                    {
                        _familyMemberDetails.Delete(familyMember);
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, $@"Family member deleted successfully.");
                    }
                    else
                    {
                        return JsonResponse.GenerateJsonResult(1, $@"Family member not exist.");
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/DeleteFamily");
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
        private string GetGender(int gender)
        {
            string GenderString = "";
            switch (gender)
            {
                case (short)Genders.Male:
                    GenderString = Genders.Male.ToString();
                    break;
                case (short)Genders.Female:
                    GenderString = Genders.Female.ToString();
                    break;
                case (short)Genders.Other:
                    GenderString = Genders.Other.ToString();
                    break;
                default:
                    break;
            }

            return GenderString;
        }
        #endregion
    }
}