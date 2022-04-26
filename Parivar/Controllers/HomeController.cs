using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Parivar.Data.DbContext;
using Parivar.Data.DbModel;
using Parivar.Dto.Enum;
using Parivar.Dto.ViewModel;
using Parivar.Models;
using Parivar.Repository.Interface;
using Parivar.Repository.Utility;
using Parivar.Utility;
using Parivar.Utility.Common;
using Parivar.Utility.Helpers;
using Parivar.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Parivar.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly IFamilyMemberDetailsService _familyMemberDetails;
        private readonly IContactUsService _contactUs;
        private readonly IFamilyUserService _familyUser;
        private readonly ICategoriesMasterService _categories;
        private readonly IRelationShipMasterService _relationShip;
        private readonly EmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly UserManager<FamilyUser> _userManager;
        private readonly IStringLocalizer<HomeController> _homeLocalizer;
        public HomeController(IContactUsService contactUs, IFamilyMemberDetailsService familyMemberDetails, IFamilyUserService familyUser,
            ICategoriesMasterService categories, IRelationShipMasterService relationShip, IStringLocalizer<HomeController> homeLocalizer,
            IOptions<EmailSettingsGmail> emailSettingsGmail, IOptions<AppSettings> appSettings, UserManager<FamilyUser> userManager)
        {
            _homeLocalizer = homeLocalizer;
            _contactUs = contactUs;
            _categories = categories;
            _familyMemberDetails = familyMemberDetails;
            _familyUser = familyUser;
            _relationShip = relationShip;
            _userManager = userManager;
            _emailService = new EmailService(emailSettingsGmail);
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddContactUs(ContactUsModel model)
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

                    var newContactUs = Mapper.Map<ContactUsModel, ContactUs>(model);
                    _contactUs.Add(newContactUs);
                    _contactUs.Save();
                    txscope.Complete();
                    return JsonResponse.GenerateJsonResult(1, "Request submitted successfully.");

                }
                catch (Exception ex)
                {
                    txscope.Dispose();
                    ErrorLog.AddErrorLog(ex, "Post/AddContactUs");
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
                }
            }
        }

        #region Family
        public IActionResult AddFamily()
        {
            ViewBag.GenderList = EnumHelpers.EnumToList<Genders>().Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() }).ToList();

            return View(new FamilyModel() { CountryId = 101, StateId = 12 });
        }

        public IActionResult AddNoOfFamilyMembers(long id)
        {
            BindDropdownList();
            List<FamilyMemberDetailsModel> familyMemberDetails = new List<FamilyMemberDetailsModel>();
            for (int i = 0; i < id; i++)
            {
                familyMemberDetails.Add(new FamilyMemberDetailsModel());
            }
            return PartialView("AddNoOfFamilyMembers", new FamilyModel() { FamilyMemberDetails = familyMemberDetails });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamily(FamilyModel model)
        {
            model.CountryId = 101;
            model.StateId = 12;
            using var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (!ModelState.IsValid) return JsonResponse.GenerateJsonResult(0, string.Join(",", ModelState.GetModelError()));
                var isExist = await _userManager.FindByEmailAsync(model.Email);
                if (isExist != null)
                {
                    txscope.Dispose();
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.AlreadyRegisterd);
                }

                #region Create New Family User
                var newFamilyUser = new FamilyUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FatherName = model.FatherName,
                    Gender = model.Gender,
                    NoOfMembers = model.NoOfMembers,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePic = FilePathList.FixProfilePic,
                    EmailConfirmed = true,
                    ResidentAddress = model.ResidentAddress,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    CountyId = model.CountyId,
                    VillageId = model.VillageId,

                    AdvanceKills = model.AdvanceKills,
                    IsActive = true
                };
                var result = await _userManager.CreateAsync(newFamilyUser, _appSettings.DefaultPassword);
                if (!result.Succeeded)
                {
                    txscope.Dispose();
                    return JsonResponse.GenerateJsonResult(0, GlobalConstant.NotRegistered);
                }
                await _userManager.AddToRoleAsync(newFamilyUser, UserRoles.FamilyMember);
                #endregion

                #region Create Family Member Details
                model.FamilyMemberDetails.ForEach(x => { x.MainMemberId = newFamilyUser.Id; x.CreatedDate = DateTime.UtcNow; });
                var familyMemberDetails = Mapper.Map<List<FamilyMemberDetailsModel>, List<FamilyMemberDetails>>(model.FamilyMemberDetails);
                familyMemberDetails.ForEach(x => x.IsActive = true);
                _familyMemberDetails.AddRange(familyMemberDetails);
                _familyMemberDetails.Save();
                #endregion

                #region Congratulation email
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(newFamilyUser);
                //var callbackUrl = Url.Action(action: nameof(AccountController.ConfirmEmail), controller: "Account", values: new { userId = newFamilyUser.Id, code }, protocol: Request.Scheme);
                //var emailTemplate = CommonMethod.ReadEmailTemplate(ErrorLog, HostingEnvironment.WebRootPath, EmailTemplateFileList.Congratulation, GetPhysicalUrl());
                //emailTemplate = emailTemplate.Replace("{name}", newFamilyUser.FullName);
                //emailTemplate = emailTemplate.Replace("{email}", newFamilyUser.Email);
                //emailTemplate = emailTemplate.Replace("{action_url}", callbackUrl);
                //await _emailService.SendEmailAsyncByGmail(new SendEmailModel
                //{
                //    ToAddress = newFamilyUser.Email,
                //    Subject = "Congratulation",
                //    BodyText = emailTemplate
                //});
                #endregion

                txscope.Complete();
                return JsonResponse.GenerateJsonResult(1, GlobalConstant.OrganisationCreated);
            }
            catch (Exception ex)
            {
                txscope.Dispose();
                ErrorLog.AddErrorLog(ex, "Post-AddFamily");
                return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
            }
        }

        [HttpGet]
        public IActionResult GetWorldDbList(long id, WorldDbs types)
        {
            List<DropdownModel> dataList = types switch
            {
                WorldDbs.Country => _familyUser.GetCountryList(),
                WorldDbs.State => _familyUser.GetStateList(id),
                WorldDbs.City => _familyUser.GetCityList(id),
                WorldDbs.District => _familyUser.GetDistrictList(id),
                WorldDbs.County => _familyUser.GetCountyList(id),
                WorldDbs.Village => _familyUser.GetVillageList(id),
                _ => _familyUser.GetCountryList(),
            };
            return JsonResponse.GenerateJsonResult(1, "", dataList);
        }

        [HttpGet]
        public IActionResult GetRelationShipList()
        {
            return JsonResponse.GenerateJsonResult(1, "", _relationShip.GetRelationShip());
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Common  
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
        private void BindDropdownList()
        {
            ViewBag.GenderList = EnumHelpers.EnumToList<Genders>().Select(x => new SelectListItem { Text = x.Name, Value = x.Value.ToString() }).ToList();
            ViewBag.BloodGroupList = _categories.GetBloodGroupList(_homeLocalizer[LocalizationConstant.BloodGroup].Value).Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Text });
            ViewBag.BusinessList = _categories.GetBusinessList(_homeLocalizer[LocalizationConstant.Business].Value).Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Text });
            ViewBag.EducationList = _categories.GetEducationList(_homeLocalizer[LocalizationConstant.EducationStudy].Value).Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Text });
            ViewBag.RelationShipList = _categories.GetRelationShipList(_homeLocalizer[LocalizationConstant.Relationship].Value).Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Text });
        }
        #endregion
    }
}
