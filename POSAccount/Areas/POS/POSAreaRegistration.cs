using System.Web.Mvc;

namespace POSAccount.Areas.POS
{
    public class POSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "POS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}