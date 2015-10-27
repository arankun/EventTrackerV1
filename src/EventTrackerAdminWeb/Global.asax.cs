#region directives

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using EventTracker.BusinessModel.Authentication;
using EventTracker.BusinessModel.Membership;
using EventTrackerAdminWeb.ModelBinders;
using Newtonsoft.Json;

#endregion

namespace EventTrackerAdminWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

           // System.Web.Mvc.ModelBinders.Binders.Add(typeof(Member), new MemberModelBinder());
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //    // Extract the forms authentication cookie
        //    string cookieName = null;
        //    cookieName = FormsAuthentication.FormsCookieName;
        //    HttpCookie authCookie = default(HttpCookie);
        //    authCookie = Context.Request.Cookies[cookieName];
        //    if ((authCookie == null))
        //    {
        //        // There is no authentication cookie.
        //        return;
        //    }
        //    FormsAuthenticationTicket authTicket = default(FormsAuthenticationTicket);
        //    authTicket = null;
        //    try
        //    {
        //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception details (omitted for simplicity)
        //        return;
        //    }
        //    //string[] roles = new string[3];
        //    //roles[0] = "One";
        //    //roles[1] = "Two";
        //    FormsIdentity id = default(FormsIdentity);
        //    id = new FormsIdentity(authTicket);
        //    CustomPrincipal principal = default(CustomPrincipal);
        //    principal = new CustomPrincipal(id);
        //    // Attach the new principal object to the current HttpContext object
        //    Context.User = principal;

        //}

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e) {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null) {

                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                var newUser = new CustomPrincipal(authTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.Roles = serializeModel.Roles;

                HttpContext.Current.User = newUser;
            }

        }

        //    //AT: DEBUGGING...REMOVE THIS METHOD

        //void Application_BeginRequest(object sender, EventArgs e) {
        //    // Auth cookie exists in the collection here! Ticket decrypts successfully
        //    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie == null)
        //        return;
        //    var encTicket = authCookie.Value;
        //   // var ticket = FormsAuthentication.Decrypt(encTicket);
        //}

        //void Application_AuthenticateRequest(object sender, EventArgs e) {
        //    //AT: DEBUGGING...REMOVE THIS METHOD

        //    // Auth cookie missing from the cookies collection here!
        //    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie == null)
        //        return;

        //    var encTicket = authCookie.Value;

        //}
    }
}
