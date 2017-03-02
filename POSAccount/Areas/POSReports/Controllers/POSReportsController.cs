using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSAccount.Areas.Reports.Controllers
{
    [RouteArea("POSReports")]
    public class POSReportsController : Controller
    {
        // GET: Reports/POSReports
        public ActionResult Index()
        {
            return View();
        }

        [Route("ChartOfAccount")]
        public ActionResult ChartOfAccount()
        {
            return View("ChartOfAccount");
        }

        [Route("Receipt")]
        public ActionResult Receipt(string DocumentNo)
        {
            //'Microsoft.Reporting.WebForms.ReportParameter'
            return View("Receipt");
        }
    }
}