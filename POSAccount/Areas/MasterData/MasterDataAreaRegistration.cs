using System.Web.Mvc;

namespace POSAccount.Areas.MasterData
{
    public class MasterDataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MasterData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}