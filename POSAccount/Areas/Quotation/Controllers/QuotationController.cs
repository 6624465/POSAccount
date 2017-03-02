using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSAccount.Areas.Quotation.Controllers
{
    [RouteArea("Quotation")]
    [SessionFilter]
    public class QuotationController : Controller
    {
        // GET: Quotation/Quotation
        public ActionResult Index()
        {
            return View();
        }

        [Route("StandardRateProfile")]
        [HttpGet]
        public ActionResult StandardRateProfile(string quotationNo)
        {
            if (quotationNo==null)
            {
                quotationNo = "STANDARD";
            }           
            var quotation = new POSAccount.BusinessFactory.QuotationBO().GetQuotation(new Contract.Quotation { QuotationNo = quotationNo });

            if (quotation==null)
        	{
                quotation = new Contract.Quotation();
                quotation.QuotationNo = quotationNo;
                quotation.CustomerCode = "NONE";
                quotation.EffectiveDate = DateTime.Today.Date;
                quotation.ExpiryDate = DateTime.Today.Date;
                quotation.QuotationDate = DateTime.Today.Date;
                quotation.QuotationItems = new List<Contract.QuotationItem>();
            }
            return View("StandardRateProfile", quotation);
        }

        [Route("SaveStandardRateProfile")]
        [HttpPost]
        public ActionResult SaveStandardRateProfile(POSAccount.Contract.Quotation Quotationdata)
        {
            try
            {
                Quotationdata.CreatedBy = Utility.DEFAULTUSER;
                Quotationdata.ModifiedBy = Utility.DEFAULTUSER;
                Quotationdata.Status = Utility.DEFAULTSTATUS;

                if (Quotationdata.QuotationItems.Count == 0 || Quotationdata.QuotationItems.Count == null)
                {
                    //Quotationdata.CompanyAddress.AddressType = "Company";
                    //Quotationdata.CompanyAddress.SeqNo = 1;
                    //Quotationdata.CompanyAddress.AddressLinkID = company.CompanyCode;

                }

                var result = new POSAccount.BusinessFactory.QuotationBO().SaveQuotation(Quotationdata);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return RedirectToAction("StandardRateProfile");
        }



        [Route("CustomerQuotations")]
        public ViewResult CustomerQuotations()
        {
            var customerQuotations = new POSAccount.BusinessFactory.QuotationBO().GetList();
            return View("CustomerQuotationList", customerQuotations);
        }


        [Route("CustomerQuotation")]
        [HttpGet]
        public ActionResult CustomerRateProfile(string quotationNo)
        {
            var quotation = new POSAccount.Contract.Quotation();

            if (quotationNo == "NEW")
                quotationNo = "";

            if (quotationNo != "" || quotationNo.Length > 0)
            {
                quotation = new POSAccount.BusinessFactory.QuotationBO().GetQuotation(new Contract.Quotation { QuotationNo = quotationNo });
            }
            else
            {
                quotation.QuotationNo = quotationNo;
                quotation.CustomerCode = "";
                quotation.EffectiveDate = DateTime.Today.Date;
                quotation.ExpiryDate = DateTime.Today.Date;
                quotation.QuotationDate = DateTime.Today.Date;
                quotation.QuotationItems = new List<QuotationItem>();
            }

            quotation.CustomerList = Utility.GetDebtorList(); 
            return View("CustomerRateProfile", quotation);
        }


        [Route("AddQuotation")]
        [HttpGet]
        public ActionResult AddQuotation(string quotationNo, Int16 itemID)
        {
            QuotationItem quotationItem = null;
            if (quotationNo == string.Empty || quotationNo == null || quotationNo == "STANDARD")
            {
                quotationItem = new QuotationItem();
            }
            else
            {
                quotationItem = new POSAccount.BusinessFactory.QuotationBO().GetQuotation(new Contract.Quotation { QuotationNo = quotationNo })
                       .QuotationItems.Where(dt => dt.ItemID == itemID).FirstOrDefault();
                if (quotationItem == null)
                {
                    quotationItem = new QuotationItem();
                }
            }
            quotationItem.ChargeCodeList = Utility.GetChargeCodeItemList();
            return PartialView("AddQuotationItem", quotationItem);
            //return PartialView("AddQuotationItem");
        }        

        [Route("CustomerQuotation")]
        [HttpPost]
        public ActionResult CustomerRateProfile(POSAccount.Contract.Quotation Quotationdata)
        {
            try
            {
                Quotationdata.CreatedBy = Utility.DEFAULTUSER;
                Quotationdata.ModifiedBy = Utility.DEFAULTUSER;
                Quotationdata.Status = Utility.DEFAULTSTATUS;

                if (Quotationdata.QuotationItems.Count == 0 || Quotationdata.QuotationItems.Count == null)
                {
                    //Quotationdata.CompanyAddress.AddressType = "Company";
                    //Quotationdata.CompanyAddress.SeqNo = 1;
                    //Quotationdata.CompanyAddress.AddressLinkID = company.CompanyCode;

                }

                var result = new POSAccount.BusinessFactory.QuotationBO().SaveQuotation(Quotationdata);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            string quotationNo = Quotationdata.QuotationNo;
            return RedirectToAction("CustomerRateProfile", new { quotationNo = quotationNo });
        }

        [Route("DeleteQuotation")]
        [HttpPost]
        public JsonResult DeleteQuotation(POSAccount.Contract.Quotation Quotationdata)
        {
            var message = string.Empty;
            var result = false;
            try
            {
                result = new POSAccount.BusinessFactory.QuotationBO().DeleteQuotation(Quotationdata);
                message = string.Empty;
                if (result)
                {
                    message = string.Format("Quotation {0} deleted successfully  ", Quotationdata.QuotationNo);
                }
                else
                {
                    message = "Unable to Delete Quotation";
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Unable to Delete Quotation.\r\n Error Code : {0} ", ex.Message);                
            }

            return Json(new { success = result, Message = message, quotationData = Quotationdata });
        }

        [Route("SaveQuotation")]
        [HttpPost]
        public JsonResult SaveQuotation(POSAccount.Contract.Quotation Quotationdata)
        {
            var message = string.Empty;
            var result = false;

            
            try
            {
                Quotationdata.CreatedBy = Utility.DEFAULTUSER;
                Quotationdata.ModifiedBy = Utility.DEFAULTUSER;
                Quotationdata.Status = Utility.DEFAULTSTATUS;
                Quotationdata.BranchID = Utility.SsnBranch;

                result = new POSAccount.BusinessFactory.QuotationBO().SaveQuotation(Quotationdata);

                if (result)
                {
                    message = string.Format("Quotation {0} saved successfully  ", Quotationdata.QuotationNo);
                }
                else
                {
                    message = "Unable to Save Quotation";
                }

            }
            catch (Exception ex)
            {

                //ModelState.AddModelError("Error", ex.Message);
                message = string.Format("Unable to Save Quotation.\r\n Error Code : {0} ", ex.Message);
            }

            return Json(new { success = result, Message = message, quotationData = Quotationdata });

        }


        [Route("DeleteQuotation")]
        public ActionResult DeleteQuotation(string QuotationItemId)
        {
            DbTransaction parentTransaction = null;
            var result = new POSAccount.BusinessFactory.QuotationItemBO().DeleteQuotationItem(QuotationItemId, parentTransaction);

            return PartialView("AddQuotationItem");
        }

    }
}