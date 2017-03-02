using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace POSAccount
{
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            if (filterContext.HttpContext.Session["BranchId"] == null && filterContext.HttpContext.Session["BranchId"] == null)
            {
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account", new { returnUrl = "" } }));
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        } 
    }
}