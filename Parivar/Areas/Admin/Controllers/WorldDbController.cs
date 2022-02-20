using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Parivar.Dto.ViewModel;
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
        private readonly IStateService _state;
        private readonly ICityService _city;
        private readonly IDistrictService _district;
        private readonly ICountyService _county;
        private readonly IVillageService _village;
        #endregion

        public WorldDbController(IFamilyUserService familyUser, ICityService city, IStateService state, IDistrictService district, ICountyService county, IVillageService village)
        {
            _familyUser = familyUser;
            _city = city;
            _state = state;
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

        [HttpGet]
        public async Task<IActionResult> GetCountryList(JQueryDataTableParamModel param)
        {
            try
            {
                var parameters = CommonMethod.GetJQueryDatatableParamList(param, GetSortingColumnName(param.iSortCol_0)).Parameters;
                var allList = await _county.GetCountryList(parameters.ToArray());
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
        public IActionResult State()
        {
            return View();
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