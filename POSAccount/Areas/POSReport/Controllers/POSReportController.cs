using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSAccount.Areas.POSReport.Controllers
{
    [RouteArea("POSReport")]
    public class POSReportController : Controller
    {
        // GET: POSReport/POSReport
        [Route("Index")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Route("TrailBalance")]
        public ActionResult TrailBalance()
        {

            var reportparam = new POSAccount.Contract.ReportParams();

            reportparam.YearList = Utility.GetYearList();

            reportparam.PeriodList = Utility.GetPeriodList();

            reportparam.AccountCodeList = Utility.GetAccountCodeItemList();
            
            return View("TrailBalance",reportparam);
        }


        [Route("BalanceSheet")]
        public ActionResult BalanceSheet()
        {

            var reportparam = new POSAccount.Contract.ReportParams();

            reportparam.YearList = Utility.GetYearList();

            reportparam.PeriodList = Utility.GetPeriodList();

            reportparam.AccountCodeList = Utility.GetAccountCodeItemList();

            return View("BalanceSheet", reportparam);
        }

        [Route("GeneralLedgerListing")]
        public ActionResult GeneralLedgerListing()
        {

            var reportparam = new POSAccount.Contract.ReportParams();

            reportparam.YearList = Utility.GetYearList();

            reportparam.PeriodList = Utility.GetPeriodList();

            reportparam.AccountCodeList = Utility.GetAccountCodeItemList();

            return View("GeneralLedgerListing", reportparam);
        }



        [Route("GLJournal")]
        public ActionResult GLJournal()
        {

            var reportparam = new POSAccount.Contract.ReportParams();

            reportparam.YearList = Utility.GetYearList();

            reportparam.PeriodList = Utility.GetPeriodList();

            reportparam.AccountCodeList = Utility.GetAccountCodeItemList();

            return View("GLJournal", reportparam);
        }



        [Route("ViewARInvoice")]
        public ActionResult ViewARInvoice(string reportSource, string DocumentId, string Url)
        {
            ViewBag.DocumentId = DocumentId;
            ViewBag.ReportSource = reportSource;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.Year = "";
            ViewBag.Period = "";
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.FromDt = "";
            ViewBag.ToDt = "";

            ViewBag.BranchID = Utility.SsnBranch;
            return PartialView("_ReportViewerPartial");
        }


        [Route("ViewWithHoldingTax")]
        public ActionResult ViewWithHoldingTax(string reportSource, string DocumentId, string Url)
        {
            ViewBag.DocumentId = DocumentId;
            ViewBag.ReportSource = reportSource;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.Year = "";
            ViewBag.Period = "";
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.FromDt = "";
            ViewBag.ToDt= "";

            ViewBag.BranchID = Utility.SsnBranch;
            return PartialView("_ReportViewerPartial");
        }
        

        [Route("ViewCBReceipt")]
        public ActionResult ViewCBReceipt(string reportSource, string DocumentId, string Url)
        {
            ViewBag.DocumentId = DocumentId;
            ViewBag.ReportSource = reportSource;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);  
            ViewBag.Year = "";
            ViewBag.Period = "";
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.FromDt = "";
            ViewBag.ToDt = "";

            ViewBag.BranchID = Utility.SsnBranch;
            return PartialView("_ReportViewerPartial");
        }

        [Route("ViewSalesReceipt")]
        public ActionResult ViewSalesReceipt(string reportSource, string DocumentId, string Url)
        {
            ViewBag.DocumentId = DocumentId;
            ViewBag.ReportSource = reportSource;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);  
            ViewBag.Year = "";
            ViewBag.Period = "";
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.FromDt = "";
            ViewBag.ToDt = "";

            ViewBag.BranchID = Utility.SsnBranch;
            return PartialView("_ReportViewerPartial");
        }
        

        [Route("ViewBalanceSheet")]
        public ActionResult ViewBalanceSheet(string year, string Url)
        {
            ViewBag.Year = year;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.ReportSource = "BalanceSheet";
            ViewBag.DocumentId = "";
            ViewBag.Period = "";
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.FromDt = DateTime.Now.Date;
            ViewBag.ToDt = DateTime.Now.Date;

            ViewBag.BranchID = Utility.SsnBranch;
            return PartialView("_ReportViewerPartial");
        }


        [Route("ViewTrailBalance")]
        public ActionResult ViewTrailBalance(string year, string period, string Url)
        {
            ViewBag.Year = year;
            ViewBag.Period = period;
            ViewBag.PeriodFrom = "";
            ViewBag.PeriodTo = "";
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.BranchID = Utility.SsnBranch;
            ViewBag.ReportSource = "TrailBalance";
            ViewBag.DocumentId = "";
            ViewBag.FromDt = DateTime.Now.Date;
            ViewBag.ToDt = DateTime.Now.Date;

            return PartialView("_ReportViewerPartial");
        }

        //var url = rooturl + "?accountCode=" + accountCode + "&fromDt=" + fromDt + "&toDt=" + toDt + "&URL=" + ReportURL;
        /*
        [Route("ViewGeneralLedgerListing")]
        public ActionResult ViewGeneralLedgerListing(string year, string periodFrom,string periodTo, string Url)
        {
            ViewBag.Year = year;
            ViewBag.Period = "";
            ViewBag.PeriodFrom = periodFrom;
            ViewBag.PeriodTo = periodTo;
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.BranchID = Utility.SsnBranch;
            ViewBag.ReportSource = "GeneralLedgerListing";
            ViewBag.DocumentId = "";
            return PartialView("_ReportViewerPartial");
        }
        */
        [Route("ViewGeneralLedgerListing")] 
        public ActionResult ViewGeneralLedgerListing(string accountCodeFrom, string accountCodeTo, string fromDt, string toDt, string Url)
        {
            //var dtFrom = Convert.ToDateTime(fromDt);
            //var dtTo = Convert.ToDateTime(toDt);

            ViewBag.AccountCode = "";
            ViewBag.AccountCodeFrom = accountCodeFrom;
            ViewBag.AccountCodeTo = accountCodeTo;
            ViewBag.Period = "";
            ViewBag.FromDt = fromDt;//dtFrom.ToString("dd/MM/yyyy"); 
            ViewBag.ToDt = toDt;//dtTo.ToString("dd/MM/yyyy");
            ViewBag.Url = string.Format("{0}{1}", Utility.REPORTSUBFOLDER, Url);
            ViewBag.BranchID = Utility.SsnBranch;
            ViewBag.ReportSource = "GeneralLedgerListing";
            ViewBag.DocumentId = "";

            return PartialView("_ReportViewerPartial");
        }
        
    }
}