using System.Web.Mvc;

namespace POSAccount.Areas.AR
{
    public class ARAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "AR_default",
            //    "AR/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.Routes.MapMvcAttributeRoutes();
        }
    }
}