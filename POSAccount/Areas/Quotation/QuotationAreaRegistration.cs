using System.Web.Mvc;

namespace POSAccount.Areas.Quotation
{
    public class QuotationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Quotation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}