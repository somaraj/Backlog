using Backlog.Core.Common;
using Backlog.Service.Authentication;
using Backlog.Service.Employees;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Employees;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Common
{
    public class AccountController : Controller
    {
        #region Fields

        protected readonly IEmployeeService _employeeService;
        protected readonly IGenericAttributeService _genericAttributeService;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly IEncryptionService _encryptionService;

        #endregion

        #region Ctor

        public AccountController(IEmployeeService employeeService,
            IGenericAttributeService genericAttributeService,
            IAuthenticationService authenticationService,
            IEncryptionService encryptionService)
        {
            _employeeService = employeeService;
            _genericAttributeService = genericAttributeService;
            _authenticationService = authenticationService;
            _encryptionService = encryptionService;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _employeeService.ValidateAsync(model.Email, model.Password);

                switch (loginResult)
                {
                    case LoginResultEnum.Successful:
                        {
                            var employee = await _employeeService.GetByEmailAsync(model.Email);
                            await _authenticationService.SignInAsync(employee, model.RememberMe);

                            var lastAccessedProjectId = await _genericAttributeService.GetAttributeAsync<int>(employee, Constant.ActiveProjectSession);
                            await HttpContext.Session.SetAsync(Constant.ActiveProjectSession, lastAccessedProjectId);

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToAction("Index", "Home");
                            return Redirect(returnUrl);
                        }
                    case LoginResultEnum.NotExist:
                        ModelState.AddModelError("", "Employee doesn't exists");
                        break;
                    case LoginResultEnum.NotActive:
                        ModelState.AddModelError("", "Employee is not active");
                        break;
                    case LoginResultEnum.Deleted:
                        ModelState.AddModelError("", "No account with this email found");
                        break;
                    case LoginResultEnum.NotRegistered:
                        ModelState.AddModelError("", "Account is not registered");
                        break;
                    case LoginResultEnum.LockedOut:
                        ModelState.AddModelError("", "Your account is locked out");
                        break;
                    case LoginResultEnum.WrongPassword:
                    default:
                        ModelState.AddModelError("", "The credentials provided are incorrect");
                        break;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Session.ClearAsync();
            await _authenticationService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Activate Account

        public async Task<IActionResult> Activate(string token)
        {
            var message = "It seems like you have clicked an invalid or expired link, please contact your administrator.";
            var decodedToken = token;
            var model = new SetPasswordModel
            {
                Token = decodedToken
            };

            if (string.IsNullOrEmpty(decodedToken))
            {
                model.Valid = false;
                model.Message = message;
                return View(model);
            }

            var employee = await _employeeService.ValidateTokenAsync(decodedToken);
            if (employee != null && employee.Id > 0)
            {
                model.Name = employee.Name;
                model.Email = employee.Email;
                model.Valid = true;
                model.Code = employee.Code;
                model.Password = null;
            }
            else
            {
                model.Valid = false;
                model.Message = message;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Activate(SetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeService.ValidateTokenAsync(model.Token);
                if (employee != null && employee.Id > 0 && employee.Code == model.Code && employee.Email == model.Email)
                {
                    await _employeeService.ResetPasswordAsync(employee.Id, model.Password);
                    return RedirectToRoute("Login");
                }
                else
                {
                    return RedirectToRoute("Activate");
                }
            }

            return View(model);
        }

        #endregion
    }
}