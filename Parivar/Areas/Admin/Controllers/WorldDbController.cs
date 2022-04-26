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
using Parivar.ViewModel;

namespace Parivar.Areas.Admin.Controllers
{
    [Authorize, Area("Admin")]
    public class WorldDbController : BaseController<WorldDbController>
    {
        private readonly IFamilyUserService _familyUser;

        #region WorldDB
        private readonly ICountryService _country;
        private readonly IStateService _state;
        private readonly ICityService _city;
        private readonly IDistrictService _district;
        private readonly ICountyService _county;
        private readonly IVillageService _village;
        #endregion

        public WorldDbController(IFamilyUserService familyUser, ICountryService country, IStateService state, ICityService city, IDistrictService district, ICountyService county, IVillageService village)
        {
            _familyUser = familyUser;
            _country = country;
            _state = state;
            _city = city;
            _district = district;
            _county = county;
            _village = village;
        }

        #region WorldDb
        public IActionResult Index()
        {
            //Task.Run(async () => await GetDataFromMapsOfIndia()).GetAwaiter().GetResult();

            return View();
        }

        #region Country
        public IActionResult Country()
        {
            return View();
        }

        public IActionResult AddEditCountry(long id)
        {
            if (id == 0) return View(@"Partial/_AddEditCountry", new CountryModel { Id = id });
            var result = _country.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditCountry", new StateModel { Id = id, Name = result.Name });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditCountry(CountryModel model)
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
                        var Country = new Countries { Name = model.Name };
                        _country.Add(Country, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Country inserted successfully.", Country.Id);
                    }
                    else
                    {
                        var result = _country.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        _country.Update(result, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Country updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditCountry");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountryList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _country.GetCountryList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetCountryList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #region State
        public IActionResult State(int id)
        {
            ViewBag.CountryId = id;
            return View();
        }

        public IActionResult AddEditState(long id, long CountryId)
        {
            var country = _country.GetSingle(x => x.Id == CountryId);
            if (id == 0) return View(@"Partial/_AddEditState", new StateModel { Id = id, CountryId = CountryId, Country = country.Name });
            var result = _state.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditState", new StateModel { Id = id, Name = result.Name, CountryId = CountryId, Country = country.Name });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditState(StateModel model)
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
                        var State = new States { Name = model.Name, CountryId = model.CountryId };

                        _state.Add(State, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "State inserted successfully.", State.Id);
                    }
                    else
                    {
                        var result = _state.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        result.CountryId = model.CountryId;
                        _state.Update(result, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "State updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditState");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStateList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _state.GetStateList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetStateList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #region City
        public IActionResult City()
        {
            return View();
        }

        public IActionResult AddEditCity(long id, long stateId)
        {
            var state = _state.GetSingle(x => x.Id == stateId);
            if (id == 0) return View(@"Partial/_AddEditCity", new CityModel { Id = id, StateId = stateId, State = state.Name });
            var result = _city.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditCity", new CityModel { Id = id, Name = result.Name, StateId = stateId, State = state.Name });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditCity(CityModel model)
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
                        var City = new Cities { Name = model.Name, StateId = model.StateId };

                        _city.Add(City, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "City inserted successfully.", City.Id);
                    }
                    else
                    {
                        var result = _city.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        result.StateId = model.StateId;
                        _city.Update(result, User.GetUserId());
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "City updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditCity");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCityList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _city.GetCityList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetCityList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #region District
        public IActionResult District()
        {
            return View();
        }

        public IActionResult AddEditDistrict(long id, long stateId)
        {
            var state = _state.GetSingle(x => x.Id == stateId);
            if (id == 0) return View(@"Partial/_AddEditDistrict", new DistrictModel { Id = id, StateId = stateId, State = state.Name });
            var result = _district.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditDistrict", new DistrictModel { Id = id, Name = result.Name, StateId = result.StateId, State = state.Name });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditDistrict(DistrictModel model)
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
                        var District = new Districts { Name = model.Name, StateId = model.StateId };

                        _district.Add(District, User.GetUserId());
                        _district.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "District inserted successfully.", District.Id);
                    }
                    else
                    {
                        var result = _district.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        result.StateId = model.StateId;
                        _district.Update(result, User.GetUserId());
                        _district.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "District updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditDistrict");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDistrictList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _district.GetDistrictList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetDistrictList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #region Countys
        public IActionResult County()
        {
            ViewBag.DistrictList = _district.GetAll(x => x.StateId == 12).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View();
        }

        public IActionResult AddEditCounty(long id)
        {
            ViewBag.DistrictList = _district.GetAll(x => x.StateId == 12).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (id == 0) return View(@"Partial/_AddEditCounty", new CountyModel { Id = id});
            var result = _county.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditCounty", new CountyModel { Id = id, Name = result.Name, DistrictId = result.DistrictId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditCounty(CountyModel model)
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
                        var County = new Countys { Name = model.Name, DistrictId = model.DistrictId };

                        _county.Add(County, User.GetUserId());
                        _county.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "County inserted successfully.", County.Id);
                    }
                    else
                    {
                        var result = _county.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        result.DistrictId = model.DistrictId;
                        _county.Update(result, User.GetUserId());
                        _county.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "County updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditCounty");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountyList(long districtId, JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                parameters.Insert(0, new SqlParameter("@DistrictId", districtId));
                var allList = await _county.GetCountyList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetCountyList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #region Village
        public IActionResult Village()
        {
            ViewBag.DistrictList = _district.GetAll(x => x.StateId == 12).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View();
        }

        public IActionResult AddEditVillage(long id)
        {
            ViewBag.DistrictList = _district.GetAll(x => x.StateId == 12).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            if (id == 0) return View(@"Partial/_AddEditVillage", new VillageModel { Id = id });
            var result = _village.GetSingle(x => x.Id == id);
            return View(@"Partial/_AddEditVillage", new VillageModel { Id = id, Name = result.Name, CountyId = result.CountyId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddEditVillage(VillageModel model)
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
                        var Village = new Villages { Name = model.Name, CountyId = model.CountyId };

                        _village.Add(Village, User.GetUserId());
                        _village.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Village inserted successfully.", Village.Id);
                    }
                    else
                    {
                        var result = _village.GetSingle(x => x.Id == model.Id);
                        result.Name = model.Name;
                        result.CountyId = model.CountyId;
                        _village.Update(result, User.GetUserId());
                        _village.Save();
                        txscope.Complete();
                        return JsonResponse.GenerateJsonResult(1, "Village updated successfully.", result.Id);
                    }
                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddEditVillage");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetVillageList(long countyId, long districtId, JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                parameters.Insert(0, new SqlParameter("@CountyId", countyId));
                parameters.Insert(1, new SqlParameter("@DistrictId", districtId));
                var allList = await _village.GetVillageList(parameters.ToArray());
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
                ErrorLog.AddErrorLog(ex, "GetVillageList");
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }
        }
        #endregion

        #endregion

        #region Common     
        private async Task GetDataFromMapsOfIndia()
        {
            using (HttpClient client = new HttpClient())
            {
                // Update port # in the following line.
                //string allState = "https://www.mapsofindia.com/villages/data.php?state=all";
                //string singleState = "https://www.mapsofindia.com/villages/data.php?state=13";
                //string allDistrict = "https://www.mapsofindia.com/villages/data.php?state=13&district=14";
                //string allVillages = "https://www.mapsofindia.com/villages/data.php?state=13&district=14&tehsil=2044";
                client.BaseAddress = new Uri("https://www.mapsofindia.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var AllState = new List<MapOfIndiaModel>();
                    var AllDistrict = new List<MapOfIndiaModel>();
                    var AllCounty = new List<MapOfIndiaModel>();
                    var AllVillages = new List<MapOfIndiaModel>();
                    HttpResponseMessage response = await client.GetAsync("villages/data.php?state=all");
                    if (response.IsSuccessStatusCode)
                    {
                        AllState = JsonConvert.DeserializeObject<List<MapOfIndiaModel>>(await response.Content.ReadAsStringAsync()); // AllState
                        var dbStates = _state.GetAll();
                        for (int i = 0; i < AllState.Count; i += 2)
                        {
                            var state = AllState[i];
                            var newState = AllState[i + 1];
                            newState.value = state.value;
                            var dbState = dbStates.FirstOrDefault(x => x.Name.ToLower() == newState.text.ToLower());
                            if (dbState == null) continue;

                            response = await client.GetAsync($@"villages/data.php?state={newState.value}");
                            if (response.IsSuccessStatusCode)
                            {
                                AllDistrict = JsonConvert.DeserializeObject<List<MapOfIndiaModel>>(await response.Content.ReadAsStringAsync());
                                for (int j = 0; j < AllDistrict.Count; j += 2)
                                {
                                    var district = AllDistrict[j];
                                    var newDistrict = AllDistrict[j + 1];
                                    newDistrict.value = district.value;

                                    if (_district.GetSingle(x => x.Name.ToLower() == newDistrict.text.ToLower()) == null)
                                    {
                                        #region Add District
                                        var addDistrict = new Data.DbModel.Districts
                                        {
                                            StateId = dbState.Id,
                                            Name = newDistrict.text,
                                        };
                                        _district.Add(addDistrict);
                                        _district.Save();
                                        #endregion
                                        response = await client.GetAsync($@"villages/data.php?state={newState.value}&district={newDistrict.value}");
                                        if (response.IsSuccessStatusCode)
                                        {
                                            AllCounty = JsonConvert.DeserializeObject<List<MapOfIndiaModel>>(await response.Content.ReadAsStringAsync());
                                            for (int k = 0; k < AllDistrict.Count; k += 2)
                                            {
                                                var county = AllCounty[k];
                                                var newCounty = AllCounty[k + 1];
                                                newCounty.value = county.value;

                                                if (_county.GetSingle(x => x.Name.ToLower() == newCounty.text.ToLower()) == null)
                                                {
                                                    #region Add County
                                                    var addCounty = new Data.DbModel.Countys
                                                    {
                                                        DistrictId = addDistrict.Id,
                                                        Name = newCounty.text,
                                                    };
                                                    _county.Add(addCounty);
                                                    _county.Save();
                                                    #endregion

                                                    response = await client.GetAsync($@"villages/data.php?state={newState.value}&district={newDistrict.value}&tehsil={newCounty.value}");
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        AllVillages = JsonConvert.DeserializeObject<List<MapOfIndiaModel>>(await response.Content.ReadAsStringAsync());
                                                        for (int l = 0; l < AllVillages.Count; l += 2)
                                                        {
                                                            var village = AllVillages[l];
                                                            var newVillage = AllVillages[l + 1];
                                                            newVillage.value = village.value;

                                                            if (_village.GetSingle(x => x.Name.ToLower() == newVillage.text.ToLower()) == null)
                                                            {
                                                                #region Add Village
                                                                var addVillage = new Data.DbModel.Villages
                                                                {
                                                                    CountyId = addCounty.Id,
                                                                    Name = newVillage.text,
                                                                };
                                                                _village.Add(addVillage);
                                                                _village.Save();
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion
    }
    public class MapOfIndiaModel
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}