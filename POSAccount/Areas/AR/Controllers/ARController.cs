using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSAccount.Contract;

namespace POSAccount.Areas.AR.Controllers
{
    [RouteArea("AR")]
    [SessionFilter]
    public class ARController : Controller
    {
        // GET: AR
        public ActionResult Index()
        {
            return View("ARInvoice");
        }


        /* Customer (Debtor) Related Action Methods */

        /* List of Customers (Debtors) */
        [Route("Customers")]
        public ActionResult CustomerList()
        {
            var lstCustomer = new POSAccount.BusinessFactory.DebtorBO().GetList();

            return View("Customers", lstCustomer);

        }

        /* Debtor Profile */
        [Route("Debtor")]
        public ActionResult DebtorProfile(string debtorCode)
        {
            var debtorBO = new POSAccount.BusinessFactory.DebtorBO();

            var debtor = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode });

            if (debtor != null)
            {
                debtor.DebtorAddress = debtorBO.GetDebtorAddress(debtor);
            }
            else
            {
                debtor = new Contract.Debtor();
                debtor.Status = true;
                debtor.DebtorCode = "";
            }
            debtor.CountryList = Utility.GetCountryList();
            debtor.DebtorAccountList = Utility.GetAccountCodeItemList();
            debtor.PaymentTypeList = Utility.GetLookupItemList("PaymentTerm");
            //return View("CompanyProfile");
            return View(debtor);

        }

        /* Save Debtor */
        [HttpPost]
        [Route("SaveDebtor")]
        public JsonResult SaveDebtor(POSAccount.Contract.Debtor debtor)
        {
            try
            {
                debtor.CreatedBy = Utility.DEFAULTUSER;
                debtor.ModifiedBy = Utility.DEFAULTUSER;
                debtor.Status = Utility.DEFAULTSTATUS;

                if (debtor.DebtorAddress.AddressId == 0 || debtor.DebtorAddress.AddressId == null)
                {
                    debtor.DebtorAddress.AddressType = "Debtor";
                    debtor.DebtorAddress.SeqNo = 1;
                    debtor.DebtorAddress.AddressLinkID = debtor.DebtorCode;

                }

                var result = new POSAccount.BusinessFactory.DebtorBO().SaveDebtor(debtor);


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }


            //return RedirectToAction("DebtorProfile");
            return Json(new { success = true, Message = "Customer saved successfully.", Debtor = debtor });

        }


        /* Delete Debtor */
        [Route("DeleteDebtor")]
        [HttpPost]
        public JsonResult DeleteDebtor(string debtorCode)
        {
            var result = false;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.DebtorBO().DeleteDebtor(new Debtor { DebtorCode = debtorCode });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Debtor deleted successfully.", debtorCode = debtorCode });

        }


        /* Search Debtor*/
        [Route("SearchDebtor")]
        public ActionResult SearchDebtor(string debtorCode)
        {
            if (debtorCode != "")
            {
                Debtor debtor = null;

                debtor = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode });

                if (debtor == null)
                {
                    debtor = new Debtor();
                }


                //}
                debtor.CountryList = Utility.GetCountryList();
                debtor.DebtorAccountList = Utility.GetDebtorAccountList();

                return RedirectToAction("DebtorProfile", new { debtorCode = debtorCode });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("DebtorProfile");
            }
        }


        public JsonResult SearchDebtorList(string searchText, int limitRecords, string source = null)
        {
            try
            {


                var result = new POSAccount.BusinessFactory.DebtorBO().GetListAutoSearch(searchText);

                return Json(result);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }



        #region AR INVOICE Action Methods

        [Route("SaveARInvoice")]
        [HttpPost]
        public JsonResult SaveARInvoice(POSAccount.Contract.ARInvoice arInvoiceData)
        {
            var recStatus = false;
            var strMessage = "";
            try
            {
                arInvoiceData.CreatedBy = Utility.DEFAULTUSER;
                arInvoiceData.ModifiedBy = Utility.DEFAULTUSER;
                arInvoiceData.Source = "AR";
                arInvoiceData.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                arInvoiceData.BranchID = Utility.SsnBranch;

                strMessage = "Invoice saved successfully.";
                var result = new POSAccount.BusinessFactory.ARInvoiceBO().SaveARInvoice(arInvoiceData);

                recStatus = result;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                strMessage = ex.Message + " >> " + ex.InnerException.Message;
            }

            return Json(new { success = recStatus, Message = strMessage, arInvoiceData = arInvoiceData });


        }


        [Route("DeleteARInvoice")]
        [HttpPost]
        public JsonResult DeleteARInvoice(string documentNo, string cancelBy)
        {
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.ARInvoiceBO().DeleteARInvoice(new ARInvoice { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Invoice deleted successfully.", documentNo = documentNo });


        }

        [Route("Invoice")]
        public ActionResult ARInvoice(string documentNo)
        {
            ARInvoice arInvoice = null;
            if (documentNo == Utility.NEWRECORD)
            {
                arInvoice = new ARInvoice();
                //arInvoice.DocumentDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));
                arInvoice.DocumentDate = DateTime.UtcNow.ThaiTime();
                arInvoice.ARInvoiceDetails = new List<ARInvoiceDetail>();
            }
            else
            {

                if (documentNo != null && documentNo != "" && documentNo != "null")
                {
                    arInvoice = new POSAccount.BusinessFactory.ARInvoiceBO().GetARInvoice(new ARInvoice { DocumentNo = documentNo });

                }
                else
                {
                    arInvoice = new ARInvoice();
                    arInvoice.DocumentDate = DateTime.Today.Date;
                    arInvoice.ARInvoiceDetails = new List<ARInvoiceDetail>();

                }

            }

            arInvoice.DebtorList = Utility.GetDebtorList();
            arInvoice.DiscountTypeList = Utility.GetDiscountList();
            arInvoice.CurrencyCodeList = Utility.GetCurrencyItemList();
            arInvoice.AccountCodeList = Utility.GetAccountCodeItemList();


            return View("ARInvoice", arInvoice);
        }

        [Route("SearchARInvoice")]
        public ActionResult SearchARInvoice(string documentNo)
        {
            if (documentNo != "0")
            {
                ARInvoice arinvoice = null;

                arinvoice = new POSAccount.BusinessFactory.ARInvoiceBO().GetARInvoice(new ARInvoice { DocumentNo = documentNo });

                if (arinvoice == null)
                {
                    arinvoice = new ARInvoice();
                    arinvoice.DocumentDate = DateTime.Today.Date;
                    arinvoice.ARInvoiceDetails = new List<ARInvoiceDetail>();
                }

                arinvoice.DebtorList = Utility.GetDebtorList();
                arinvoice.CurrencyCodeList = Utility.GetCurrencyItemList();


                return RedirectToAction("ARInvoice", new { InvoiceType = "", DocumentNo = documentNo });

            }
            else
            {
                return RedirectToAction("ARInvoice");
            }
        }


        [Route("AddARInvoiceItem")]
        [HttpGet]
        public ActionResult AddARInvoiceItem(string documentNo, Int16 itemNo)
        {
            ARInvoiceDetail arInvoicedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                arInvoicedetail = new ARInvoiceDetail();
            }
            else
            {
                arInvoicedetail = new POSAccount.BusinessFactory.ARInvoiceBO().GetARInvoice(new Contract.ARInvoice { DocumentNo = documentNo })
                       .ARInvoiceDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (arInvoicedetail == null)
                {
                    arInvoicedetail = new ARInvoiceDetail();
                }
            }

            arInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            arInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return PartialView("AddARInvoiceItem", arInvoicedetail);
            //return PartialView("AddQuotationItem");
        }




        //[Route("AddARInvoiceItem")] 
        //[HttpPost]
        //public ActionResult AddARInvoiceItem(string debtorCode,string accountCode, decimal amount)
        //{
        //    ARInvoiceDetail item = new ARInvoiceDetail();

        //    item.AccountCode = accountCode;

        //    item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID= Utility.SsnBranch }).Description;
        //    item.BaseAmount = amount;
        //    item.LocalAmount = amount;
        //    item.CurrencyCode = "THB";
        //    item.TotalAmount = amount;
        //    item.Discount = 0;
        //    item.DiscountType = "NONE";

        //    var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode }).DebtorAccount;


        //    //arInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
        //    //arInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

        //    return Json(new { Message = "Success", ARInvoiceDetail = item,DebtorAccount = debtorAccount }, JsonRequestBehavior.AllowGet);
        //}


        [Route("AddARInvoiceItem")]
        [HttpPost]
        public ActionResult AddARInvoiceItem(string debtorCode, string accountCode, decimal amount, string bankCode = "")
        {
            ARInvoiceDetail item = new ARInvoiceDetail();

            item.AccountCode = accountCode;

            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            item.BaseAmount = amount;
            item.LocalAmount = amount;
            item.CurrencyCode = "THB";
            item.TotalAmount = amount;
            item.Discount = 0;
            item.DiscountType = "NONE";

            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = debtorCode }).DebtorAccount;

            string bankAccount = "";

            if (!string.IsNullOrWhiteSpace(bankCode))
            {
                bankAccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode }).BankAccount;
            }
            var whAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARWHACCOUNTCODE);
            var vatAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARVATACCOUNTCODE);
            //arInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            //arInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(debtorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            string bankDC = "";
            if (!string.IsNullOrWhiteSpace(bankDC))
            {
                debitCreditObj = Utility.GetAccountType(bankAccount);
                bankDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            }

            debitCreditObj = Utility.GetAccountType(whAccount);
            string whDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(vatAccount);
            string vatDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(accountCode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */

            return Json(new
            {
                Message = "Success",
                ARInvoiceDetail = item,
                DebtorAccount = debtorAccount,
                bankAccount = bankAccount,
                whAccount = whAccount,
                vatAccount = vatAccount,
                customerDebitCredit = customerDC,
                bankDebitCredit = bankDC,
                whDebitCredit = whDC,
                vatDebitCredit = vatDC,
                accountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);


            //arInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            //arInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //return Json(new { Message = "Success", ARInvoiceDetail = item,DebtorAccount = debtorAccount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchItem(string searchText, int limitRecords, string source = null)
        {
            try
            {
                IQueryable<ARInvoice> _iQCBReceipt;
                _iQCBReceipt = new POSAccount.BusinessFactory.ARInvoiceBO().GetList().AsQueryable();//.GetARInvoice(searchText, limitRecords);
                //var res = (from r in result where r.BranchID.ToString().Contains(searchText) select r.BranchID).ToList();

                return Json(new { data = _iQCBReceipt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }



        [Route("ARInvoiceList")]
        [HttpGet]
        public ActionResult ARInvoiceList()
        {
            var lstARInvoices = new POSAccount.BusinessFactory.ARInvoiceBO().GetList().Where(x=>x.IsCancel==false).ToList();
            return View("ARInvoiceList", lstARInvoices);


        }

        #endregion

        #region AR CREDIT NOTE


        [Route("SaveARCreditNote")]
        [HttpPost]
        public JsonResult SaveARCreditNote(POSAccount.Contract.ARCreditNote ARCreditNoteData)
        {
            try
            {
                ARCreditNoteData.CreatedBy = Utility.DEFAULTUSER;
                ARCreditNoteData.ModifiedBy = Utility.DEFAULTUSER;
                ARCreditNoteData.Source = "";


                var result = new POSAccount.BusinessFactory.ARCreditNoteBO().SaveARCreditNote(ARCreditNoteData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "Credit Note saved successfully.", ARCreditNoteData = ARCreditNoteData });


        }


        [Route("DeleteARCreditNote")]
        [HttpPost]
        public JsonResult DeleteARCreditNote(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.ARCreditNoteBO().DeleteARCreditNote(new ARCreditNote { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Credit Note deleted successfully.", documentNo = documentNo });


        }

        [Route("CreditNote")]
        public ActionResult ARCreditNote(string invoiceType, string documentNo)
        {
            ARCreditNote ARCreditNote = null;
            if (invoiceType == Utility.NEWRECORD)
            {
                ARCreditNote = new ARCreditNote();
                ARCreditNote.DocumentDate = DateTime.UtcNow.ThaiTime();
                ARCreditNote.ARCreditNoteDetails = new List<ARCreditNoteDetail>();

                ARCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();
            }
            else
            {

                if (documentNo != null && documentNo != "")
                {
                    ARCreditNote = new POSAccount.BusinessFactory.ARCreditNoteBO().GetARCreditNote(new ARCreditNote { DocumentNo = documentNo });
                    ARCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();
                }
                else
                {
                    //ARInvoice = new POSAccount.BusinessFactory.ARInvoiceBO().GetList().FirstOrDefault();

                    ARCreditNote = new ARCreditNote();
                    ARCreditNote.DocumentDate = DateTime.Today.Date;
                    ARCreditNote.ARCreditNoteDetails = new List<ARCreditNoteDetail>();
                    ARCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();
                }



            }
            ARCreditNote.DebtorList = Utility.GetDebtorList();
            ARCreditNote.CurrencyCodeList = Utility.GetCurrencyItemList();
            ARCreditNote.DebtorAccountList = Utility.GetDebtorAccountList();


            return View("ARCreditNote", ARCreditNote);
        }

        [HttpPost]
        public JsonResult AddARCreditNote(string accountcode, string amount, string customerCode)
        {
            ARCreditNoteDetail item = new ARCreditNoteDetail();
            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountcode, BranchID = Utility.SsnBranch }).Description;
            item.AccountCode = accountcode;
            item.BaseAmount = Convert.ToDecimal(amount);
            item.LocalAmount = Convert.ToDecimal(amount);
            item.CurrencyCode = "THB";
            item.TaxAmount = 0;
            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = customerCode }).DebtorAccount;
            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(debtorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            debitCreditObj = Utility.GetAccountType(accountcode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */
            return Json(new
            {
                Message = "Success",
                ARCreditNoteDetail = item,
                DebtorAccount = debtorAccount,
                CustomerDebitCredit = customerDC,
                AccountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);
        }


        [Route("SearchARCreditNote")]
        public ActionResult SearchARCreditNote(string documentNo)
        {
            if (documentNo != "0")
            {
                ARCreditNote ARCreditNote = null;

                ARCreditNote = new POSAccount.BusinessFactory.ARCreditNoteBO().GetARCreditNote(new ARCreditNote { DocumentNo = documentNo });

                if (ARCreditNote == null)
                {
                    ARCreditNote = new ARCreditNote();
                    ARCreditNote.DocumentDate = DateTime.Today.Date;
                    ARCreditNote.ARCreditNoteDetails = new List<ARCreditNoteDetail>();
                }


                return RedirectToAction("ARCreditNote", new { InvoiceType = "", DocumentNo = documentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("ARCreditNote");
            }
        }


        [Route("AddARCreditNoteItem")]
        [HttpGet]
        public ActionResult AddARCreditNoteItem(string documentNo, Int16 itemNo)
        {
            ARCreditNoteDetail ARCreditNotedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                ARCreditNotedetail = new ARCreditNoteDetail();
            }
            else
            {
                ARCreditNotedetail = new POSAccount.BusinessFactory.ARCreditNoteBO().GetARCreditNote(new Contract.ARCreditNote { DocumentNo = documentNo })
                       .ARCreditNoteDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (ARCreditNotedetail == null)
                {
                    ARCreditNotedetail = new ARCreditNoteDetail();
                }
            }

            ARCreditNotedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            ARCreditNotedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return PartialView("AddARCreditNoteItem", ARCreditNotedetail);
            //return PartialView("AddQuotationItem");
        }
        #endregion

        #region AR DEBIT NOTE


        [Route("SaveARDebitNote")]
        [HttpPost]
        public JsonResult SaveARDebitNote(POSAccount.Contract.ARDebitNote ARDebitNoteData)
        {
            try
            {
                ARDebitNoteData.CreatedBy = Utility.DEFAULTUSER;
                ARDebitNoteData.ModifiedBy = Utility.DEFAULTUSER;
                ARDebitNoteData.Source = "";


                var result = new POSAccount.BusinessFactory.ARDebitNoteBO().SaveARDebitNote(ARDebitNoteData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "Debit Note saved successfully.", ARDebitNoteData = ARDebitNoteData });


        }


        [Route("DeleteARDebitNote")]
        [HttpPost]
        public JsonResult DeleteARDebitNote(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.ARDebitNoteBO().DeleteARDebitNote(new ARDebitNote { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Debit Note deleted successfully.", documentNo = documentNo });


        }

        [Route("DebitNote")]
        public ActionResult ARDebitNote(string invoiceType, string documentNo)
        {
            ARDebitNote ARDebitNote = null;
            if (invoiceType == Utility.NEWRECORD)
            {
                ARDebitNote = new ARDebitNote();
                ARDebitNote.DocumentDate = DateTime.UtcNow.ThaiTime();
                ARDebitNote.ARDebitNoteDetails = new List<ARDebitNoteDetail>();

            }
            else
            {

                if (documentNo != null && documentNo != "" && documentNo != "null")
                {
                    ARDebitNote = new POSAccount.BusinessFactory.ARDebitNoteBO().GetARDebitNote(new ARDebitNote { DocumentNo = documentNo });

                }
                else
                {
                    //ARInvoice = new POSAccount.BusinessFactory.ARInvoiceBO().GetList().FirstOrDefault();

                    ARDebitNote = new ARDebitNote();
                    ARDebitNote.DocumentDate = DateTime.Today.Date;
                    ARDebitNote.ARDebitNoteDetails = new List<ARDebitNoteDetail>();

                }
            }

            ARDebitNote.DebtorList = Utility.GetDebtorList();
            ARDebitNote.CurrencyCodeList = Utility.GetCurrencyItemList();
            ARDebitNote.DebtorAccountList = Utility.GetDebtorAccountList();
            ARDebitNote.AccountCodeList = Utility.GetAccountCodeItemList();

            return View("ARDebitNote", ARDebitNote);
        }

        [Route("SearchARDebitNote")]
        public ActionResult SearchARDebitNote(string documentNo)
        {
            if (documentNo != "0")
            {
                ARDebitNote ARDebitNote = null;

                ARDebitNote = new POSAccount.BusinessFactory.ARDebitNoteBO().GetARDebitNote(new ARDebitNote { DocumentNo = documentNo });

                if (ARDebitNote == null)
                {
                    ARDebitNote = new ARDebitNote();
                    ARDebitNote.DocumentDate = DateTime.Today.Date;
                    ARDebitNote.ARDebitNoteDetails = new List<ARDebitNoteDetail>();
                }

                return RedirectToAction("ARDebitNote", new { InvoiceType = "", DocumentNo = documentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("ARDebitNote");
            }
        }


        [Route("AddARDebitNoteItem")]
        [HttpGet]
        public ActionResult AddARDebitNoteItem(string documentNo, Int16 itemNo)
        {
            ARDebitNoteDetail ARDebitNotedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                ARDebitNotedetail = new ARDebitNoteDetail();
            }
            else
            {
                ARDebitNotedetail = new POSAccount.BusinessFactory.ARDebitNoteBO().GetARDebitNote(new Contract.ARDebitNote { DocumentNo = documentNo })
                       .ARDebitNoteDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (ARDebitNotedetail == null)
                {
                    ARDebitNotedetail = new ARDebitNoteDetail();
                }
            }

            ARDebitNotedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            ARDebitNotedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return PartialView("AddARDebitNoteItem", ARDebitNotedetail);
            //return PartialView("AddQuotationItem");
        }


        [HttpPost]
        public JsonResult AddARDebitNote(string accountcode, string amount, string customerCode)
        {
            ARDebitNoteDetail item = new ARDebitNoteDetail();
            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountcode, BranchID = Utility.SsnBranch }).Description;
            item.AccountCode = accountcode;
            item.BaseAmount = Convert.ToDecimal(amount);
            item.LocalAmount = Convert.ToDecimal(amount);
            item.CurrencyCode = "THB";
            item.TaxAmount = 0;
            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = customerCode }).DebtorAccount;
            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(debtorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            debitCreditObj = Utility.GetAccountType(accountcode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */
            return Json(new
            {
                Message = "Success",
                ARDebitNoteDetail = item,
                DebtorAccount = debtorAccount,
                CustomerDebitCredit = customerDC,
                AccountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);
            //return Json(new { Message = "Success", ARDebitNoteDetail = item, DebtorAccount = debtorAccount }, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}