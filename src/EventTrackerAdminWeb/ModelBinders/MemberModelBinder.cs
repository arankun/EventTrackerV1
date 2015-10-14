using System.Web.Mvc;
using EventTracker.BusinessModel.Membership;

namespace EventTrackerAdminWeb.ModelBinders
{
    public class MemberModelBinder:IModelBinder
    {
        private const string SessionKey = "Member";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Member memberContext = (controllerContext.HttpContext.Session != null) ? (controllerContext.HttpContext.Session[SessionKey] as Member) : null;

            if (memberContext == null) {
                memberContext = new Member();
                controllerContext.HttpContext.Session[SessionKey] = memberContext;
            }

            return memberContext;
        }
    }
}