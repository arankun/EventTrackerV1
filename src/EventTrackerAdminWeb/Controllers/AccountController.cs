#region directives

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EventTracker.BusinessModel.Authentication;
using EventTracker.BusinessServices.Authentication;
using Newtonsoft.Json;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _service;

        public AccountController(IAuthenticationService service)
        {
            _service = service;
        }

        // GET: Account
        public ActionResult Index()
        {
            if (TempData["LoginError"] != null) {
                ViewBag.LoginError = TempData["LoginError"];
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult LoginAuthentication(LoginViewModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid) return View("Index");

            TempData["LoginError"] = "";

            var error = string.Empty;
            var appUser = _service.AuthenticateUser(model.Username, model.Password);

            if (appUser == null) {
                TempData["LoginError"] = "Error Authenticating. Please enter a valid Login ID and Password.";
                return RedirectToAction("Index", "Account");
            }
            else {
                //FormsAuthentication.SetAuthCookie(appUser.Username, false);
                var userData = JsonConvert.SerializeObject(appUser);
                var authTicket = new FormsAuthenticationTicket(
                         version:1,
                         name:appUser.Username,
                         issueDate: DateTime.Now,
                         expiration:DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                         isPersistent:true,
                         userData:userData);

                var encTicket = FormsAuthentication.Encrypt(authTicket);
                var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

                if (appUser.Roles.Contains("Admin")) {
                    return RedirectToAction("Index", "Membership");
                }
                else if (appUser.Roles.Contains("User")) {
                    return RedirectToAction("Index", "Events");
                }
                else {
                    return RedirectToAction("Index", "Home");
                }


                //******************
                //string userData = JsonConvert.SerializeObject(appUser);
                //var authTicket = new FormsAuthenticationTicket(
                //1,
                //appUser.Username,
                //DateTime.Now,
                //DateTime.Now.AddMinutes(30),
                //false, //pass here true, if you want to implement remember me functionality
                //userData);

                //// Encrypt the ticket.
                //string encTicket = FormsAuthentication.Encrypt(authTicket);

                //// Create the cookie.
                //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                //return RedirectToAction("Index", "Events");
            }
        }
    }
}