using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.Areas.GL.Controllers
{
    [RouteArea("GL")]
    [SessionFilter]
    public class GLController : Controller
    {
        // GET: GL
        public ActionResult Index()
        {
            return View("GLJournalEntry");
        }

        [Route("GLEntry")]
        public ActionResult GLJournalEntry(string documentNo)
        {
            GLJournal gljournal = new GLJournal();
            if (documentNo == Utility.NEWRECORD)
            {
                
                gljournal.DocumentDate = DateTime.UtcNow.Date.ThaiTime();
                gljournal.GLJournalDetails = new List<GLJournalDetail>();
            }
            else
            {
                gljournal = new POSAccount.BusinessFactory.GLJournalBO().GetGLJournal(new GLJournal { BranchID = Utility.SsnBranch, DocumentNo = documentNo });

            }
            gljournal.AccountCodeList = Utility.GetAccountCodeItemList();
            gljournal.DebitCreditList = Utility.GetLookupItemList("DebitCredit");

            return View("GLJournalEntry", gljournal);
        }



        [Route("SaveGLJournal")]
        public JsonResult SaveGLJournal(POSAccount.Contract.GLJournal glJournalData)
        {
            try
            {
                glJournalData.CreatedBy = Utility.DEFAULTUSER;
                glJournalData.ModifiedBy = Utility.DEFAULTUSER;
                glJournalData.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                glJournalData.Source = "GL";
                glJournalData.DocumentDate = DateTime.UtcNow.ThaiTime();
                glJournalData.BranchID = Utility.SsnBranch;

                var result = new POSAccount.BusinessFactory.GLJournalBO().SaveGLJournal(glJournalData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "GL Entry saved successfully.", Data = glJournalData });

        }




        [Route("FinancialPeriod")]
        public ActionResult GLFinancialPeriod()
        {
            var branchID = Utility.SsnBranch;

            GLFinancialPeriod glFinancialPeriod = new GLFinancialPeriod();
            List<GLFinancialPeriod> lstGLFinancialPeriod = new List<GLFinancialPeriod>();

            FinancialPeriodListVM FinancialPeriodList = new FinancialPeriodListVM();
            FinancialPeriodList.GLFinancialPeriod = lstGLFinancialPeriod;
            //glTransaction.AccountCodeList = Utility.GetAccountCodeItemList();
            //glTransaction.DebitCreditList = Utility.GetLookupItemList("DebitCredit");
            //glTransaction.DocumentDate = DateTime.Now.Date;
            //glTransaction.GLJournalDetails = new List<GLJournalDetail>();

            return View("FinancialPeriod", FinancialPeriodList);
        }

        [Route("GetFinancialPeriod")]
        public ActionResult GetFinancialPeriod(string year)
        {
            if (Utility.SsnBranch != null)
            {
                FinancialPeriodListVM FinancialPeriodList = new FinancialPeriodListVM();
                var _year = Convert.ToInt32(year);
                short branchid = 30;// Utility.SsnBranch;
                var financialPeriodList = new NetStock.BusinessFactory.GLFinancialPeriodBO().GetList(branchid, _year);
                FinancialPeriodList.GLFinancialPeriod = financialPeriodList;
                return View("FinancialPeriod", FinancialPeriodList);
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [Route("SaveExtendedFinancialPeriod")]
        public ActionResult SaveExtendedFinancialPeriod(FinancialPeriodListVM GLFinancialPeriodList)
        {
            //GLFinancialPeriod GLFinancialPeriod = new GLFinancialPeriod();
            //var saveGLFinPeriod = new NetStock.BusinessFactory.GLFinancialPeriodBO().SaveGLFinancialPeriod(GLFinancialPeriod);

            GLFinancialPeriodList.GLFinancialPeriod.Update(dt =>
            {
                dt.BranchID = Utility.SsnBranch;

            });

            var saveGLFinPeriod = new NetStock.BusinessFactory.GLFinancialPeriodBO().SaveGLFinancialPeriod(GLFinancialPeriodList.GLFinancialPeriod);

            return RedirectToAction("GLFinancialPeriod");

        }




        [HttpPost]
        public JsonResult GetCreditType(string accountCode)
        {
            var accountType = Utility.GetAccountType(accountCode);
            return Json(accountType, JsonRequestBehavior.AllowGet);
        }

        [Route("AddGLJournalDetails")]
        [HttpPost]
        public ActionResult AddGLJournalDetails(string accountCode, string amount, string remarks)
        {

            GLJournalDetail glJournalDetails = new GLJournalDetail();

            glJournalDetails.AccountCode = accountCode;
            glJournalDetails.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            var debitCreditObj = Utility.GetAccountType(accountCode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            glJournalDetails.Remark = remarks;
            if (accountDC == "CREDIT")
            {
                glJournalDetails.BaseCreditAmount = Convert.ToDecimal(amount);
            }
            else
            {
                glJournalDetails.BaseDebitAmount = Convert.ToDecimal(amount);
            }

            return Json(new { Message = "Success", GLJournalDetails = glJournalDetails, DebitCredit = accountDC }, JsonRequestBehavior.AllowGet);
        }


        [Route("AddGLTransactionsDetails")]
        [HttpPost]
        public ActionResult AddTransactionsDetails(string accountCode, string amount, string remarks, string debitcredit)
        {
            GLTransaction glTransaction = new GLTransaction();
            glTransaction.AccountCode = accountCode;
            glTransaction.Remark = remarks;

            if (debitcredit == "CREDIT")
            {
                glTransaction.CreditAmount = Convert.ToDecimal(amount);
            }
            else
            {
                glTransaction.DebitAmount = Convert.ToDecimal(amount);
            }

            return Json(new { Message = "Success", GLTransactionDetails = glTransaction }, JsonRequestBehavior.AllowGet);
        }



        [Route("GLOpening")]
        public ActionResult GLOpening(string documentNo)
        {
            GLOpening glOpening = new GLOpening();

            glOpening.AccountCodeList = Utility.GetAccountCodeItemList();
            glOpening.DebitCreditList = Utility.GetLookupItemList("DebitCredit");
            glOpening.AccountDate = DateTime.Now.Date;
            glOpening.BranchID = Utility.SsnBranch;
            glOpening.GLOpenDetails = new List<GLOpeningDetail>();



            //glTransaction.GLJournalDetails = new List<GLJournalDetail>();

            return View("GLOpening", glOpening);
        }

        [Route("AddGLOpeningDetails")]
        [HttpPost]
        public ActionResult AddGLOpeningDetails(string accountCode, string amount)
        {

            GLOpeningDetail glOpeningDetails = new GLOpeningDetail();
            glOpeningDetails.AccountCode = accountCode;
            glOpeningDetails.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            var debitCreditObj = Utility.GetAccountType(accountCode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            if (accountDC == "CREDIT")
            {
                glOpeningDetails.CreditAmount = Convert.ToDecimal(amount);
            }
            else
            {
                glOpeningDetails.DebitAmount = Convert.ToDecimal(amount);
            }

            return Json(new { Message = "Success", GLOpeningDetails = glOpeningDetails, DebitCredit = accountDC }, JsonRequestBehavior.AllowGet);
        }

        [Route("SaveGLOpening")]
        public JsonResult SaveGLOpening(POSAccount.Contract.GLOpening glOpeningData)
        {
            try
            {
                glOpeningData.CreatedBy = Utility.DEFAULTUSER;
                glOpeningData.ModifiedBy = Utility.DEFAULTUSER;
                glOpeningData.AccountDate = DateTime.Now;
                glOpeningData.BranchID = Utility.SsnBranch;


                if (glOpeningData.GLOpenDetails.Count > 0)
                {
                    glOpeningData.TotalCreditAmount = glOpeningData.GLOpenDetails.Sum(dt => dt.CreditAmount);
                    glOpeningData.TotalDebitAmount = glOpeningData.GLOpenDetails.Sum(dt => dt.DebitAmount);
                }


                var result = new POSAccount.BusinessFactory.GLOpeningBO().SaveGLOpening(glOpeningData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "GL Entry saved successfully.", Data = glOpeningData });

        }

        [Route("DeleteGLOpening")]
        public JsonResult DeleteGLOpening(POSAccount.Contract.GLOpening glOpeningData)
        {
            try
            {
                glOpeningData.CreatedBy = Utility.DEFAULTUSER;
                glOpeningData.ModifiedBy = Utility.DEFAULTUSER;
                glOpeningData.AccountDate = DateTime.Now;
                glOpeningData.BranchID = Utility.SsnBranch;


                if (glOpeningData.GLOpenDetails.Count > 0)
                {
                    glOpeningData.TotalCreditAmount = glOpeningData.GLOpenDetails.Sum(dt => dt.CreditAmount);
                    glOpeningData.TotalDebitAmount = glOpeningData.GLOpenDetails.Sum(dt => dt.DebitAmount);
                }


                var result = new POSAccount.BusinessFactory.GLOpeningBO().DeleteGLOpening(glOpeningData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "GL Entry deleted successfully.", Data = glOpeningData });


        }

        [Route("JVManual")]
        public ActionResult JVManual(string documentNo)
        {
            GLJournal gljournal = new GLJournal();
            if (documentNo == Utility.NEWRECORD)
            {
                gljournal.DocumentDate = DateTime.Now.Date;
                gljournal.GLJournalDetails = new List<GLJournalDetail>();
            }
            else
            {
                gljournal = new POSAccount.BusinessFactory.GLJournalBO().GetGLJournal(new GLJournal { BranchID = Utility.SsnBranch, DocumentNo = documentNo });

            }
            gljournal.AccountCodeList = Utility.GetAccountCodeItemList();
            gljournal.DebitCreditList = Utility.GetLookupItemList("DebitCredit");
                
            return View("JVManual", gljournal);
        }

        [Route("AddJVManualDetails")]
        public ActionResult AddJVManualDetails(string accountCode, string amount, string remarks,string debitcredit)
        {

            GLJournalDetail glJournalDetails = new GLJournalDetail();

            glJournalDetails.AccountCode = accountCode;
            glJournalDetails.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            //var debitCreditObj = Utility.GetAccountType(accountCode);
            //string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            glJournalDetails.Remark = remarks;
            if (debitcredit == "CREDIT")
            {
                glJournalDetails.BaseCreditAmount = Convert.ToDecimal(amount);
            }
            else
            {
                glJournalDetails.BaseDebitAmount = Convert.ToDecimal(amount);
            }

            return Json(new { Message = "Success", GLJournalDetails = glJournalDetails, DebitCredit = debitcredit }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("SaveJVManualDetails")]
        public JsonResult SaveJVManualDetails(POSAccount.Contract.GLJournal glJournalData)
        {
            try
            {
                glJournalData.CreatedBy = Utility.DEFAULTUSER;
                glJournalData.ModifiedBy = Utility.DEFAULTUSER;
                glJournalData.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                glJournalData.Source = "JV";
                //glJournalData.DocumentDate = DateTime.UtcNow.ThaiTime();
                glJournalData.BranchID = Utility.SsnBranch;

                var result = new POSAccount.BusinessFactory.GLJournalBO().SaveGLJournal(glJournalData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "JV Manual Entry saved successfully.", Data = glJournalData });

            
            
        }

        [HttpPost]
        [Route("DeleteJVData")]
        public JsonResult DeleteJVData(POSAccount.Contract.GLJournal glJournalData)
        {
            try
            {
                glJournalData.CreatedBy = Utility.DEFAULTUSER;
                glJournalData.ModifiedBy = Utility.DEFAULTUSER;
                glJournalData.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                glJournalData.Source = "GL";
                glJournalData.DocumentDate = DateTime.Now;
                glJournalData.BranchID = Utility.SsnBranch;

                var result = new POSAccount.BusinessFactory.GLJournalBO().DeleteGLJournal(glJournalData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "JV Manual Entry deleted successfully.", Data = glJournalData });
        }


        [Route("GLJournalList")] 
        [HttpGet]
        public ActionResult GLJournalList()
        {
            var lstGLJournal = new POSAccount.BusinessFactory.GLJournalBO().GetList(Utility.SsnBranch).Where(x => x.IsCancel == false && x.Source=="GL").ToList();
            return View("GLJournalListing", lstGLJournal);
        }

        [Route("JVManualList")]
        [HttpGet]
        public ActionResult JVManualList()
        {
            var lstGLJournal = new POSAccount.BusinessFactory.GLJournalBO().GetList(Utility.SsnBranch).Where(x => x.IsCancel == false && x.Source == "JV").ToList();
            return View("JVManualListing", lstGLJournal);


        }

    }
    public class FinancialPeriodListVM
    {
        public List<GLFinancialPeriod> GLFinancialPeriod { get; set; }
    }



}
