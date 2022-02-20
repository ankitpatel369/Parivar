using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parivar.Repository.Exceptions;
using Parivar.Repository.Interface;
using Parivar.Utility;
using Parivar.ViewModel;

namespace Parivar.Areas.Member.Controllers
{
    [Authorize, Area("Member")]
    public class MyAccountController : BaseController<MyAccountController>
    {
        private readonly IFamilyMemberDetailsService _familyMemberDetails;
        private readonly IFamilyUserService _familyUser;

        public MyAccountController(IFamilyMemberDetailsService familyMemberDetails, IFamilyUserService familyUser)
        {
            _familyMemberDetails = familyMemberDetails;
            _familyUser = familyUser;
        }

        public IActionResult Index()
        {
            ViewBag.IsBreadcrumb = true;
            var mainMember = _familyUser.GetById(User.GetUserId());
            var userProfileInfo = new ProfileViewModel()
            {
                Id = mainMember.Id,
                FullName = mainMember.FullName,
                Gender = mainMember.Gender,
                AdvanceSkills = mainMember.AdvanceKills,
                Email = mainMember.Email,
                Mobile = mainMember.PhoneNumber,
                Location = mainMember.ResidentAddress
            };
            return View(userProfileInfo);
        }
    }
}
