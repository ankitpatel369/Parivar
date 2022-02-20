using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Parivar.Data.DbModel;
using Parivar.Dto.ViewModel;
using Parivar.Repository.Exceptions;
using Parivar.Repository.Interface;
using Parivar.Utility;
using Parivar.Utility.Common;
using Parivar.Utility.Extension;
using Parivar.Utility.Helpers;
using Parivar.ViewModel;

namespace Parivar.Areas.Admin.Controllers
{
    [Authorize, Area("Admin")]
    public class CategoriesController : BaseController<CategoriesController>
    {
        private readonly ICategoriesMasterService _categories;

        public CategoriesController(ICategoriesMasterService categories)
        {
            _categories = categories;
        }

        public IActionResult Index()
        {
            ViewBag.CategoryTypeList = EnumHelpers.EnumToList<Categories>().Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesList(JQueryDataTableParamModel param, int categoryType = 0)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                parameters.Insert(0, new SqlParameter("@CategoryType", categoryType));
                var allList = await _categories.GetCategoriesMasterList(parameters.ToArray());

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
                ErrorLog.AddErrorLog(ex, "GetCategoriesList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }

        // GET: Admin/Categories/AddEditCategories
        [HttpGet]
        public IActionResult AddEditCategories(long id = 0, int selectedCategory = 0)
        {
            ViewBag.CategoryTypeList = EnumHelpers.EnumToList<Categories>().Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() }).ToList();

            var edit = _categories.GetSingle(x => x.Id == id);
            if (edit == null) return PartialView(@"Partial/_AddEditCategories", new CategoriesModel { Id = 0, Categories = (Categories)selectedCategory });

            var model = new CategoriesModel { Id = edit.Id, CategoryName = edit.CategoryName, Categories = (Categories)edit.Categories, IsActive = edit.IsActive };
            return PartialView(@"Partial/_AddEditCategories", model);
        }

        // POST: Admin/Categories/AddEditCategories
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditCategories(CategoriesModel model)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        txscope.Dispose();
                        return JsonResponse.GenerateJsonResult(0, string.Join(",", ModelState.GetModelError()));
                    }
                    if (model.Id == 0)
                    {
                        var isExist = _categories.GetCount(x => x.CategoryName.ToLower().Equals(model.CategoryName.ToLower()) && x.Categories == (int)model.Categories);
                        if (isExist > 0)
                        {
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "Categories already exists.");
                        }
                        _categories.Add(new CategoriesMaster
                        {
                            CategoryName = model.CategoryName,
                            Categories = (int)model.Categories,
                            IsActive = true
                        }, User.GetUserId());
                        _categories.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Categories created successfully.");
                    }
                    else
                    {

                        var isExist = _categories.GetCount(x => x.CategoryName.ToLower().Equals(model.CategoryName.ToLower()) && x.Categories == (int)model.Categories && x.Id != model.Id);
                        if (isExist > 0)
                        {
                            txscope.Dispose();
                            return JsonResponse.GenerateJsonResult(0, "Categories already exists.");
                        }
                        var edit = _categories.GetSingle(x => x.Id == model.Id);
                        edit.CategoryName = model.CategoryName;
                        edit.Categories = (int)model.Categories;
                        edit.IsActive = model.IsActive;
                        _categories.Update(edit, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Categories updated successfully.");
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditCategories");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpPost]
        public IActionResult ManageCategoriesStatus(long id)
        {
            using (TransactionScope txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _categories.GetSingle(x => x.Id == id);
                    result.IsActive = !result.IsActive;
                    _categories.Update(result, User.GetUserId());
                    txscope.Complete();
                    return JsonResponse.GenerateJsonResult(1, $@"Categories {(result.IsActive ? "activated" : "deactivated")} successfully.", result.Id);
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/ManageCategoriesStatus");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpPost]
        public IActionResult RemoveCategories(long id)
        {
            using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = _categories.GetSingle(x => x.Id == id);
                    _categories.Delete(result);
                    _categories.Save();
                    txscope.Complete();
                    return JsonResponse.GenerateJsonResult(1, @"Categories deleted successfully.");
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/RemoveCategories");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }
    }
}