using System.Web.Mvc;

namespace POSAccount.Areas.AP
{
    public class APAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "AP_default",
            //    "AP/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.Routes.MapMvcAttributeRoutes();
        }
    }
}