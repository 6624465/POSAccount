using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSAccount.Contract;
using POSAccount.BusinessFactory;

namespace POSAccount.Areas.CB.Controllers
{
    [RouteArea("CB")]
    [SessionFilter]
    public class CBController : Controller
    {
        // GET: CB/CB



        public ActionResult Index()
        {
            return View();
        }

        [Route("BankProfileList")]
        [HttpGet]
        public ActionResult BankProfileList()
        {
            var lstbanks = new POSAccount.BusinessFactory.BankBO().GetList();
            //HttpContext.Session["OrderItems"] = null;
            return View("BankProfileList", lstbanks);


        }

        [Route("BankProfile")]
        public ActionResult BankProfile(string bankCode)
        {
            Bank bankprofile = null;
            var bankBo = new POSAccount.BusinessFactory.BankBO();
            if (bankCode == Utility.NEWRECORD)
            {
                bankprofile = new Bank();
            }
            else
            {
                bankprofile = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode });
            }
            bankprofile.BankAccountList = Utility.GetAccountGroupItemList(Utility.SsnBranch);
            bankprofile.CurrencyList = Utility.GetCurrencyItemList();
            bankprofile.CountryList = Utility.GetCountryList();

            return View("BankProfile", bankprofile);
        }

        [HttpPost]
        [Route("BankProfile")]
        public ActionResult SaveBankProfile(POSAccount.Contract.Bank bank)
        {

            try
            {
                bank.CreatedBy = Utility.DEFAULTUSER;
                bank.ModifiedBy = Utility.DEFAULTUSER;
                bank.Status = Utility.DEFAULTSTATUS;
                

                if (bank.BankAddress.AddressId == 0 || bank.BankAddress.AddressId == null)
                {
                    bank.BankAddress.AddressType = "Bank";
                    bank.BankAddress.SeqNo = 1;
                    bank.BankAddress.AddressLinkID = bank.BankCode;

                }
                var result = new POSAccount.BusinessFactory.BankBO().SaveBank(bank);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error", ex.Message);
            }
            
            //return RedirectToAction("BankProfile");// "Bank details Saved.";
            return Json(new { success = true, Message = "Bank Profile saved successfully." });
        }


        #region CBPAYMENT Action Methods


        [Route("CBPaymentList")]
        [HttpGet]
        public ActionResult CBPaymentList()
        {
            var lstCBPayment = new POSAccount.BusinessFactory.CBPaymentBO().GetList().Where(x=> x.IsCancel==false).ToList();
            return View("CBPaymentList", lstCBPayment);


        }


        [Route("CBPayment")]
        public ActionResult CBPayment(string CBPaymentType, string DocumentNo)
        {
            CBPayment cbpayment = null;

            //DocumentNo = DocumentNo ?? Utility.NEWRECORD;

            if (DocumentNo == Utility.NEWRECORD)
            {
                cbpayment = new CBPayment();
                //cbpayment.AccountDate =null;
                cbpayment.DocumentDate = DateTime.UtcNow.ThaiTime();
                cbpayment.AccountDate = DateTime.UtcNow.ThaiTime();
                cbpayment.ChequeDate = DateTime.UtcNow.ThaiTime();

                cbpayment.CBPaymentDetails = new List<CBPaymentDetail>();
                cbpayment.CBPaymentSetOffDetails = new List<CBPaymentSetOffDetail>();
                cbpayment.CBPaymentGLDetails = new List<CBPaymentGlDetail>();
            }
            else
            {
                var cbpaymentBO = new POSAccount.BusinessFactory.CBPaymentBO();
                //var cbpayment1 = new POSAccount.BusinessFactory.CBPaymentBO().GetList();
                cbpayment = new POSAccount.BusinessFactory.CBPaymentBO().GetCBPayment(new CBPayment { DocumentNo = DocumentNo });

                if (cbpayment == null)
                {
                    cbpayment = new CBPayment();
                }

                cbpayment.CBPaymentSetOffDetails = new List<CBPaymentSetOffDetail>();
                cbpayment.CBPaymentGLDetails = new List<CBPaymentGlDetail>();

                if (cbpayment.PaymentType == "CREDITOR")
                {

                }
                else if (cbpayment.PaymentType == "PAY-OTHERS")
                {
                    cbpayment.CBPaymentDetails = new POSAccount.BusinessFactory.CBPaymentDetailBO().GetListByDocumentNo(cbpayment.DocumentNo);
                    cbpayment.GLTransactionDetails = new POSAccount.BusinessFactory.GLTransactionBO().GetList(cbpayment.DocumentNo);
                    
                }

            }
            
            cbpayment.PaymentTypeList = Utility.GetPaymentTypeList("PaymentType");
            cbpayment.CreditorList = Utility.GetCreditorList();
            cbpayment.BankCodeList = Utility.GetBankCodeList();
            cbpayment.CurrencyCodeList = Utility.GetCurrencyItemList();

            //cbpayment.CBPaymentDetails = new List<CBPaymentDetail>();
            return View("CBPayment", cbpayment);
        }



        [Route("AddCBPaymentItem")]
        public PartialViewResult AddCBPaymentItem(string documentNo, Int16 itemNo)
        {
            CBPaymentDetail cbPaymentDetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                cbPaymentDetail = new CBPaymentDetail();
                cbPaymentDetail.SetOffDate = DateTime.Now;
            }
            else
            {
                cbPaymentDetail = new POSAccount.BusinessFactory.CBPaymentBO().GetCBPayment(new Contract.CBPayment { DocumentNo = documentNo })
                       .CBPaymentDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (cbPaymentDetail == null)
                {
                    cbPaymentDetail = new CBPaymentDetail();
                }
            }

            cbPaymentDetail.CurrencyList = Utility.GetCurrencyItemList();
            cbPaymentDetail.AccountCodeList = Utility.GetAccountCodeItemList();
            cbPaymentDetail.ChargeCodeList = Utility.GetChargeCodeItemList();
            cbPaymentDetail.SetOffDate = DateTime.Now;
            return PartialView("AddChargeCode", cbPaymentDetail);
            //return PartialView("AddQuotationItem");
        }


        [Route("SaveCBPayment")]
        [HttpPost]
        public JsonResult SaveCBPayment(POSAccount.Contract.CBPayment CBPaymentdata)
        {
            try
            {
                CBPaymentdata.CreatedBy = Utility.DEFAULTUSER;
                CBPaymentdata.ModifiedBy = Utility.DEFAULTUSER;
                CBPaymentdata.Source = "CB";
                CBPaymentdata.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                CBPaymentdata.BranchID = Utility.SsnBranch;


                if (CBPaymentdata.CBPaymentDetails != null)
                {
                    foreach (var item in CBPaymentdata.CBPaymentDetails)
                    {
                        item.MatchDocumentDate = DateTime.UtcNow.ThaiTime();
                    }

                }

                var result = new POSAccount.BusinessFactory.CBPaymentBO().SaveCBPayment(CBPaymentdata);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "CBPayment saved successfully.", CBPaymentData = CBPaymentdata });

            //return RedirectToAction("CBPayment");
        }


        [Route("DeleteCBPayment")]
        [HttpPost]
        public JsonResult DeleteCBPayment(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.CBPaymentBO().DeleteCBPayment(documentNo, cancelBy);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Payment deleted successfully.", documentNo = documentNo });

            //return RedirectToAction("CBReceipt", new { CBReceiptType = "", DocumentNo = CBReceiptdata.DocumentNo });
        }


        [Route("CBSearchPayment")]
        public ActionResult CBSearchPayment(string DocumentNo)
        {
            if (DocumentNo != "0")
            {
                CBPayment cbPayment = null;

                var cbpaymentBO = new POSAccount.BusinessFactory.CBPaymentBO();
                var c = new POSAccount.BusinessFactory.CBPaymentBO().GetList();
                cbPayment = new POSAccount.BusinessFactory.CBPaymentBO().GetList().Where(x => x.DocumentNo == DocumentNo).FirstOrDefault();

                if (cbPayment == null)
                {
                    cbPayment = new CBPayment();
                }
                if (cbPayment.DocumentNo != null)
                {
                    cbPayment.CBPaymentDetails = new POSAccount.BusinessFactory.CBPaymentDetailBO().GetListByDocumentNo(cbPayment.DocumentNo);
                }
                else
                {
                    cbPayment.CBPaymentDetails = new List<CBPaymentDetail>();
                }

                //}
                cbPayment.DocumentDate = DateTime.Today.Date;
                cbPayment.PaymentTypeList = Utility.GetReceiptTypeList("PaymentType");
                cbPayment.CreditorList = Utility.GetCreditorList();
                cbPayment.BankCodeList = Utility.GetBankCodeList();
                cbPayment.CurrencyCodeList = Utility.GetCurrencyItemList();
                return RedirectToAction("CBPayment", new { CBPaymentType = "", DocumentNo = DocumentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("CBPayment");
            }
        }

        [Route("GetCreditorOutStandingDocuments")]
        public JsonResult GetCreditorOutStandingDocuments(string creditorCode, string bankCode, string matchDocumentNo)
        {
            var cbpaymentsetoffdetails = new POSAccount.DataFactory.CBPaymentSetOffDetailDAL().GetCreditorOutStandingDocuments(creditorCode, matchDocumentNo);

            var bankaccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode }).BankAccount;

            var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = creditorCode }).CreditorAccount;
            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(creditorAccount);
            var creditorDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(bankaccount);
            var bankDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(Utility.BANKCHARGESACCOUNTCODE);
            var bankChargesDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */

            return Json(new {
                SetOffDetailsData = cbpaymentsetoffdetails,
                CreditorAccount = creditorAccount,
                BankAccount = bankaccount,
                CreditorDebitCredit = creditorDC,
                bankDebitCredit = bankDC,
                bankChargesDebitCredit = bankChargesDC
            }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region CB RECEIPT Action Methods

        [Route("CBReceiptList")]
        [HttpGet]
        public ActionResult CBReceiptList()
        {
            var lstCBReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetList().Where(x=> x.IsCancel==false).ToList();
            return View("CBReceiptList", lstCBReceipt);


        }




        [Route("GetBankChargeDBAccountCode")]
        public JsonResult GetBankChargeDBAccountCode(string customerCode)
        {
            var bankChargeAccount = Utility.BANKCHARGESACCOUNTCODE;
            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = customerCode }).DebtorAccount;
            return Json(new { bankChargeAccount = bankChargeAccount, debtorAccount = debtorAccount }, JsonRequestBehavior.AllowGet);
        }


        [Route("GetBankChargeAccountCode")]
        public JsonResult GetBankChargeAccountCode(string customerCode)
        {
            var bankChargeAccount = Utility.BANKCHARGESACCOUNTCODE;
            var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = customerCode }).CreditorAccount;

            var debitCreditObj = Utility.GetAccountType(creditorAccount);
            string customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(Utility.BANKCHARGESACCOUNTCODE);
            string bankChargesDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            return Json(new {
                bankChargeAccount = bankChargeAccount,
                creditorAccount = creditorAccount,
                creditorDebitCredit = customerDC,
                bankChargesDebitCredit = bankChargesDC
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("SaveCBReceipt")]
        [HttpPost]
        public JsonResult SaveCBReceipt(POSAccount.Contract.CBReceipt CBReceiptdata)
        {
            try
            {
                CBReceiptdata.CreatedBy = Utility.DEFAULTUSER;
                CBReceiptdata.ModifiedBy = Utility.DEFAULTUSER;
                CBReceiptdata.Source = "CB";
                CBReceiptdata.CurrencyCode = "THB";
                CBReceiptdata.BranchID = Utility.SsnBranch;

                if (CBReceiptdata.CBReceiptDetails!=null)
                {
                    foreach (var item in CBReceiptdata.CBReceiptDetails)
                    {
                        item.MatchDocumentDate = DateTime.UtcNow.ThaiTime();
                    }
                }

                var result = new POSAccount.BusinessFactory.CBReceiptBO().SaveCBReceipt(CBReceiptdata);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "CBReceipt saved successfully.", CBReceiptdata = CBReceiptdata });

            //return RedirectToAction("CBReceipt", new { CBReceiptType = "", DocumentNo = CBReceiptdata.DocumentNo });
        }


        [Route("DeleteCBReceipt")]
        [HttpPost]
        public JsonResult DeleteCBReceipt(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                //CBReceiptdata.CreatedBy = Utility.DEFAULTUSER;
                //CBReceiptdata.ModifiedBy = Utility.DEFAULTUSER;
                //CBReceiptdata.Source = "";
                result = new POSAccount.BusinessFactory.CBReceiptBO().DeleteCBReceipt(documentNo, cancelBy);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "CBReceipt deleted successfully.", documentNo = documentNo });

            //return RedirectToAction("CBReceipt", new { CBReceiptType = "", DocumentNo = CBReceiptdata.DocumentNo });
        }

        [HttpGet]
        [Route("CBReceipt")]
        public ActionResult CBReceipt(string CBReceiptType, string DocumentNo)
        {
            CBReceipt cbReceipt = null;
            if (DocumentNo == Utility.NEWRECORD)
            {
                cbReceipt = new CBReceipt();
                cbReceipt.DocumentDate = DateTime.UtcNow.ThaiTime();
                cbReceipt.AccountDate = DateTime.UtcNow.ThaiTime();
                cbReceipt.ChequeDate = DateTime.UtcNow.ThaiTime();
                //cbpayment.AccountDate =null;
                cbReceipt.CBReceiptDetails = new List<CBReceiptDetail>();
                cbReceipt.CBReceiptSetOffDetails = new List<CBReceiptSetOffDetail>();
                cbReceipt.CBReceiptGLDetails = new List<CBReceiptGlDetail>();
            }
            else
            {
                var cbpaymentBO = new POSAccount.BusinessFactory.CBReceiptBO();
                if (DocumentNo != null && DocumentNo != "")
                {
                    //cbReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetList().Where(x => x.DocumentNo == DocumentNo).FirstOrDefault();

                    cbReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetCBReceipt(new CBReceipt { BranchID = Utility.SsnBranch, DocumentNo = DocumentNo });

                }
                else
                {
                    cbReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetList().FirstOrDefault();
                
                    cbReceipt.CBReceiptDetails = new List<CBReceiptDetail>();
                    cbReceipt.CBReceiptSetOffDetails = new List<CBReceiptSetOffDetail>();
                    cbReceipt.CBReceiptGLDetails = new List<CBReceiptGlDetail>();
                }
                

            }
            
            cbReceipt.ReceiptTypeList = Utility.GetReceiptTypeList("ReceiptType");
            cbReceipt.DebtorList = Utility.GetDebtorList();
            cbReceipt.BankCodeList = Utility.GetBankCodeList();
            cbReceipt.CurrencyCodeList = Utility.GetCurrencyItemList();
            return View("CBReceipt", cbReceipt);
        }

        [Route("CBSearchReceipt")]
        public ActionResult CBSearchReceipt(string DocumentNo)
        {
            if (DocumentNo != "0")
            {
                CBReceipt cbReceipt = null;

                var cbpaymentBO = new POSAccount.BusinessFactory.CBReceiptBO();
                var c = new POSAccount.BusinessFactory.CBReceiptBO().GetList();
                cbReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetList().Where(x => x.DocumentNo == DocumentNo).FirstOrDefault();

                if (cbReceipt == null)
                {
                    cbReceipt = new CBReceipt();
                }
                if (cbReceipt.DocumentNo != null)
                {
                    cbReceipt.CBReceiptDetails = new POSAccount.BusinessFactory.CBReceiptDetailBO().GetListByDocumentNo(cbReceipt.DocumentNo);
                    cbReceipt.CBReceiptSetOffDetails = new List<CBReceiptSetOffDetail>();
                    cbReceipt.CBReceiptGLDetails = new List<CBReceiptGlDetail>();
                }
                else
                {
                    cbReceipt.CBReceiptDetails = new List<CBReceiptDetail>();
                    cbReceipt.CBReceiptSetOffDetails = new List<CBReceiptSetOffDetail>();
                    cbReceipt.CBReceiptGLDetails = new List<CBReceiptGlDetail>();
                }

                //}
                cbReceipt.DocumentDate = DateTime.Today.Date;
                cbReceipt.ReceiptTypeList = Utility.GetReceiptTypeList("ReceiptType");
                cbReceipt.DebtorList = Utility.GetDebtorList();
                cbReceipt.BankCodeList = Utility.GetBankCodeList();
                cbReceipt.CurrencyCodeList = Utility.GetCurrencyItemList();
                return RedirectToAction("CBReceipt", new { CBReceiptType = "", DocumentNo = DocumentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("CBReceipt");
            }
        }


        public JsonResult SearchItem(string searchText, int limitRecords, string source = null)
        {
            try
            {
                IQueryable<CBReceipt> _iQCBReceipt;
                _iQCBReceipt = new POSAccount.BusinessFactory.CBReceiptBO().GetList().AsQueryable();//.GetARInvoice(searchText, limitRecords);
                //var res = (from r in result where r.BranchID.ToString().Contains(searchText) select r.BranchID).ToList();

                return Json(new { data = _iQCBReceipt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }


        [Route("AddCBReceiptItem")]
        [HttpGet]
        public PartialViewResult AddCBReceiptItem(string documentNo, Int16 itemNo)
        {
            CBReceiptDetail cbReceiptDetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                cbReceiptDetail = new CBReceiptDetail();
                cbReceiptDetail.SetOffDate = DateTime.Now;
            }
            else
            {
                cbReceiptDetail = new POSAccount.BusinessFactory.CBReceiptBO().GetCBReceipt(new Contract.CBReceipt { DocumentNo = documentNo })
                       .CBReceiptDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (cbReceiptDetail == null)
                {
                    cbReceiptDetail = new CBReceiptDetail();
                }
            }

            cbReceiptDetail.CurrencyList = Utility.GetCurrencyItemList();
            cbReceiptDetail.AccountCodeList = Utility.GetAccountCodeItemList();
            cbReceiptDetail.ChargeCodeList = Utility.GetChargeCodeItemList();
            return PartialView("AddChargeCodeReceipt", cbReceiptDetail);
            //return PartialView("AddQuotationItem");
        }

        //[Route("GetDebtorOutStandingDocuments")]
        //public JsonResult GetDebtorOutStandingDocuments(string debtorCode, string bankCode, string matchDocumentNo)
        //{
        //    var cbreceiptsetoffdetails = new POSAccount.DataFactory.CBReceiptSetOffDetailDAL().GetDebtorOutStandingDocuments(debtorCode, matchDocumentNo);

        //    var bankaccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode }).BankAccount;

        //    var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode}).DebtorAccount;


        //    return Json(new { SetOffDetailsData = cbreceiptsetoffdetails }, JsonRequestBehavior.AllowGet);

        //}

        [Route("GetDebtorOutStandingDocuments")]
        public JsonResult GetDebtorOutStandingDocuments(string debtorCode, string bankCode, string matchDocumentNo)
        {
            var cbreceiptsetoffdetails = new POSAccount.DataFactory.CBReceiptSetOffDetailDAL().GetDebtorOutStandingDocuments(debtorCode, matchDocumentNo);

            var bankaccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode }).BankAccount;

            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode }).DebtorAccount;


            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(debtorAccount);
            var creditorDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(bankaccount);
            var bankDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */

            return Json(new
            {
                SetOffDetailsData = cbreceiptsetoffdetails,
                DebtorAccount = debtorAccount,
                BankAccount = bankaccount,
                CreditorDebitCredit = creditorDC,
                bankDebitCredit = bankDC
            }, JsonRequestBehavior.AllowGet);

        }


       

        #endregion



        #region BANKTRANSFER Action Methods



        [Route("BankTransfer")]
        public ActionResult BankTransfer()
        {
            var branchID = Convert.ToInt16(Session["BranchId"]);
            CBBankTransfer cbBankTransfer = new CBBankTransfer();
            cbBankTransfer.BankCodeList = Utility.GetBankCodeList();
            cbBankTransfer.ChequeDate = DateTime.Now;
            cbBankTransfer.DocumentDate = DateTime.Now;

            return View(cbBankTransfer);
        }


        [Route("SaveBankTransfer")]
        [HttpPost]
        public JsonResult SaveBankTransfer(POSAccount.Contract.CBBankTransfer CBBankTransfer)
        {

            try
            {
                CBBankTransfer.BranchID = Utility.SsnBranch;
                CBBankTransfer.CreatedBy = Utility.DEFAULTUSER;
                CBBankTransfer.ModifiedBy = Utility.DEFAULTUSER;
                CBBankTransfer.Source = "CB";

                var result = new POSAccount.BusinessFactory.CBBankTransferBO().SaveCBBankTransfer(CBBankTransfer);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "CBPayment saved successfully.", CBBankTransfer = CBBankTransfer });
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetBankAccount(string bankcode)
        {
            var bankAccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankcode }).BankAccount;
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            string accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAPWHTaxAccount()
        {
            var bankAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.APWHACCOUNTCODE);
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            string accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAPVatTaxAccount()
        {
            var bankAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.APVATACCOUNTCODE);
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            string accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region CB Bank Reconciliation

        [Route("CBBankReconciliation")]
        public ActionResult CBBankReconciliation()
        {
            BankRecon bankRecon = new BankRecon();
            var branchID = Convert.ToInt16(Session["BranchId"]);
            bankRecon.StartDate = DateTime.Now;
            bankRecon.EndDate = DateTime.Now;
            bankRecon.BankCodeList = Utility.GetBankCodeList();
            return View(bankRecon);
        }



        [Route("GetBankReconDetails")]
        [HttpPost]
        public ActionResult GetBankReconDetails(BankReconVm Obj)
        {
            try
            {

            var branchID = Convert.ToInt16(Session["BranchId"]);
            //var endDtUtc = Obj.endDate.ToUniversalTime().ThaiTime();
            var bankReconDetails = new POSAccount.BusinessFactory.BankBO().GetBankReconciliationList(branchID, Obj.bankCode, Obj.startDate.ToUniversalTime().ThaiTime(), Obj.endDate.ToUniversalTime().ThaiTime());
            return PartialView("_BankReconDetails", bankReconDetails);

            }
            catch (Exception ex)
            {

                return Json(ex.Message + "  >> " + ex.InnerException.Message.ToString());
            }
            
        }

        #endregion


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetCBRecAPWHTaxAccount()
        {
            var bankAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARWHACCOUNTCODE);
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            var accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetCBRecAPVatTaxAccount()
        {
            var bankAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARVATACCOUNTCODE);
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            var accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAdvanceVATAccountCode()
        {
            var bankAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARADVANCEVATACCOUNTCODE);
            var debitCreditObj = Utility.GetAccountType(bankAccount);
            var accountType = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            return Json(new { bankAccount = bankAccount, accountType = accountType }, JsonRequestBehavior.AllowGet);
        }

        

    }
}

public class BankReconVm
{
    public string bankCode { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}
