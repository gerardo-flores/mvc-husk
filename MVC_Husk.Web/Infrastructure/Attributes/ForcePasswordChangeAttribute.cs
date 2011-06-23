//
//  Copyright Info
//

using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using MVC_Husk.Controllers;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Model;

namespace MVC_Husk.Infrastructure.Attributes
{
    public class ForcePasswordChangeAttribute : ActionFilterAttribute
    {
        [Inject]
        public IIdStore IdStore { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.Controller is AccountController || filterContext.Controller is HomeController))
            {
                string id = IdStore.GetUserId();

                if (id == null)
                {
                    return;
                }
                else
                {
                    if (new Users().Single(id).RequiredPasswordChange)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "ChangePassword" }, { "controller", "Account" } });
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
}