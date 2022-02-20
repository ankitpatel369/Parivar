using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Parivar.Data.DbContext;
using Parivar.Dto.Enum;
using Parivar.Dto.ViewModel;
using Parivar.Models;
using Parivar.Repository.Exceptions;
using Parivar.Repository.Interface;
using Parivar.Repository.Utility;
using Parivar.Utility;
using Parivar.Utility.Common;
using Parivar.Utility.Extension;

namespace Parivar.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : BaseController<AccountController>
    {
        private readonly UserManager<FamilyUser> _userManager;
        private readonly SignInManager<FamilyUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly EmailService _emailService;
        private readonly IErrorLogService _errorLog;
        private readonly IFamilyUserService _user;
        private readonly ILogger _logger;
        private ISession Session => Accessor.HttpContext.Session;

        public AccountController(UserManager<FamilyUser> userManager, SignInManager<FamilyUser> signInManager, RoleManager<Role> roleManager,
            IOptions<EmailSettingsGmail> emailSettingsGmail, IFamilyUserService user, IErrorLogService errorLog, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailService = new EmailService(emailSettingsGmail);
            _errorLog = errorLog;
            _user = user;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                //var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
                //var userRole = GetUserRoles(user).FirstOrDefault();
                if (User.IsInRole(UserRoles.SystemAdmin))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Dashboard", new { area = "" });
            }
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.HideSignIn = true;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", GlobalConstant.InvalidLogin);
                return View(model);
            }

            var userRole = GetUserRoles(user).FirstOrDefault();
            if (!userRole.NullToString().Equals(UserRoles.SystemAdmin))
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, GlobalConstant.ConfirmEmail);
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, GlobalConstant.AccountDeactivated);
                    return View(model);
                }
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                user.LastLogin = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);
                if (userRole.Contains(UserRoles.SystemAdmin))
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                if (userRole.Contains(UserRoles.FamilyMember))
                    return RedirectToAction("Index", "Dashboard", new { area = "Member" });

            }
            if (result.RequiresTwoFactor)
                return RedirectToAction(nameof(LoginWith2Fa), new { returnUrl, model.RememberMe });
            if (result.IsLockedOut)
                return RedirectToAction(nameof(Lockout));

            ModelState.AddModelError(string.Empty, GlobalConstant.InvalidLogin);
            return View(model);

            // If we got this far, something failed, redisplay form
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2FaViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
            ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion

        #region Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new FamilyUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = "";// Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailService.SendEmailAsyncByGmail(new SendEmailModel()
                    {
                        ToAddress = model.Email,
                        BodyText = callbackUrl
                    });

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Confirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new FamilyUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            if (user.EmailConfirmed)
            {
                TempData["Message"] = @"Your email is already confirmed.";
                return RedirectToAction("ConfirmEmailConfirmation", "Account");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded) return View("Error");
            var userRole = GetUserRoles(user).FirstOrDefault();
            if (userRole == null)
            {
                TempData["Message"] = @"Your email is confirmed.";
                return RedirectToAction("ConfirmEmailConfirmation", "Account");
            }
            return RedirectToAction("ConfirmEmailConfirmation", "Account");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailConfirmation()
        {
            TempData.Keep("Message");
            return View();
        }
        #endregion

        #region Forgot & Reset Password
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = GlobalConstant.EmailNotFound;
                return RedirectToAction(nameof(ForgotPassword));
            }
            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Message"] = GlobalConstant.ConfirmEmail;
                return RedirectToAction(nameof(ForgotPassword));
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = "";// Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            string emailTemplate = CommonMethod.ReadEmailTemplate(_errorLog, HostingEnvironment.WebRootPath, EmailTemplateFileList.ResetPassword, GetPhysicalUrl());
            emailTemplate = emailTemplate.Replace("{UserName}", user.FullName);
            emailTemplate = emailTemplate.Replace("{action_url}", callbackUrl);

            await _emailService.SendEmailAsyncByGmail(new SendEmailModel
            {
                ToAddress = user.Email,
                Subject = "Reset Password",
                BodyText = emailTemplate
            });

            TempData["Message"] = @"Please check your email to reset your password.";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }

            var userId = Accessor.HttpContext.Request.Query["userId"];
            var user = await _userManager.FindByIdAsync(userId);
            var model = new ResetPasswordViewModel { Code = code, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["Message"] = @"Your password does not reset successfully.";
                return RedirectToAction(nameof(Login));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                TempData["Message"] = @"Your password reset successfully.";
                return RedirectToAction(nameof(Login));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.HideLogin = true;
            return View();
        }

        #region Global Methods
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CheckUserIsExist(string email)
        {
            email = email.Trim();
            var isExist = _user.GetSingle(x => x.UserName.Equals(email) || x.UserName.Equals(email) || x.NormalizedEmail.Equals(email));
            return JsonResponse.GenerateJsonResult(isExist != null ? 1 : 0, isExist != null ? GlobalConstant.AlreadyRegisterd : "");
        }

        [HttpGet]
        public async Task<IActionResult> GlobalResetPassword(long id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var role = GetUserRoles(user).FirstOrDefault();
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return JsonResponse.GenerateJsonResult(0, "Don't reveal that the user is not confirmed");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = "";// Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                string emailTemplate = CommonMethod.ReadEmailTemplate(_errorLog, HostingEnvironment.WebRootPath, EmailTemplateFileList.ResetPassword, GetPhysicalUrl());
                emailTemplate = emailTemplate.Replace("{action_url}", callbackUrl);
                emailTemplate = emailTemplate.Replace("{UserName}", user.FullName);
                await _emailService.SendEmailAsyncByGmail(new SendEmailModel
                {
                    ToAddress = user.Email,
                    Subject = "Reset Password",
                    BodyText = emailTemplate
                });
                return JsonResponse.GenerateJsonResult(1, $@"User password link sent successfully");
            }
            catch (Exception ex)
            {
                _errorLog.AddErrorLog(ex, "GET/ResetPassword");
                return JsonResponse.GenerateJsonResult(0, GlobalConstant.SomethingWrong);
            }
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IList<string> GetUserRoles(FamilyUser user)
        {
            var rolesList = Task.Run(() => _userManager.GetRolesAsync(user)).Result;
            return rolesList;
        }

        #endregion
    }
}
