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

namespace Parivar.Areas.Member.Controllers
{
    [Authorize, Area("Member")]
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
            return View();
        }

        [Route("/Member/FamilyList")]
        public IActionResult FamilyList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFamilyMemberList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0));
                parameters.Parameters.Insert(0, new SqlParameter("@MainMemberId", SqlDbType.BigInt) { Value = User.GetUserId() });

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
        [Route("/Member/Family/{id}")]
        public IActionResult ViewFamily(long id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetFamilyChartData()
        {
            try
            {
                var chartData = new List<FamilyMemberDetailsForChartModel>();
                var mainFamilyMember = _familyUser.GetById(User.GetUserId());
                chartData.Add(new FamilyMemberDetailsForChartModel
                {
                    Id = mainFamilyMember.Id,
                    FullName = mainFamilyMember.FullName,
                    RelationShip = "Main Member",
                    Gender = mainFamilyMember.Gender.HasValue ? mainFamilyMember.Gender.Value : 0
                }) ;
                chartData.AddRange(mainFamilyMember.FamilyMemberDetails.Select(x => new FamilyMemberDetailsForChartModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    RelationShip = x.RelationShip.CategoryName,
                    MainMemberId = x.MainMemberId,
                    MainMemberName = x.MainMember.FullName,
                    Gender = x.Gender.HasValue ? x.Gender.Value : 0 
                }));

                return JsonResponse.GenerateJsonResult(1, "success", chartData);
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex, "GetFamilyList");
                return JsonResponse.GenerateJsonResult(0, "Something went wrong.");
            }
        }
        #endregion
    }
}