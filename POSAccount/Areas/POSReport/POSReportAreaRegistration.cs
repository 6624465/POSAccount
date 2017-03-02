    using System.Web.Mvc;

namespace POSAccount.Areas.POSReport
{
    public class POSReportAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "POSReport";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "POSReport_default",
            //    "POSReport/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}