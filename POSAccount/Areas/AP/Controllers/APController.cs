using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POSAccount.Areas.AP.Controllers
{
    [RouteArea("AP")]
    [SessionFilter]
    public class APController : Controller
    {        
        public ActionResult Index()
        {
            return View("APInvoice");
        }


        #region SUPPLIER/CREDITOR

        
        [Route("Suppliers")]
        public ActionResult SupplierList()
        {
            var lstSupplier = new POSAccount.BusinessFactory.CreditorBO().GetList();

            return View("Suppliers", lstSupplier);

        }


        [Route("SupplierProfile")]
        public ActionResult CreditorProfile(string creditorCode)
        {
            var creditorBO = new POSAccount.BusinessFactory.CreditorBO();

            var creditor = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = creditorCode });

            if (creditor != null)
            {
                creditor.CreditorAddress = creditorBO.GetCreditorAddress(creditor);
            }
            else
            {
                creditor = new Contract.Creditor();
                creditor.Status = true;
                creditor.CreditorCode = "";
            }
            creditor.CountryList = Utility.GetCountryList();
            creditor.CreditorAccountList = Utility.GetAccountCodeItemList();
            creditor.PaymentTypeList = Utility.GetLookupItemList("PaymentTerm");
            
            return View("AddSupplier",creditor);

        }

        /* Save creditor */
        [HttpPost]
        [Route("Savecreditor")]
        public JsonResult Savecreditor(POSAccount.Contract.Creditor creditor)
        {
            try
            {
                creditor.CreatedBy = Utility.DEFAULTUSER;
                creditor.ModifiedBy = Utility.DEFAULTUSER;
                creditor.Status = Utility.DEFAULTSTATUS;

                if (creditor.CreditorAddress.AddressId == 0 || creditor.CreditorAddress.AddressId == null)
                {
                    creditor.CreditorAddress.AddressType = "Creditor";
                    creditor.CreditorAddress.SeqNo = 1;
                    creditor.CreditorAddress.AddressLinkID = creditor.CreditorCode;

                }

                var result = new POSAccount.BusinessFactory.CreditorBO().SaveCreditor(creditor);


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }


            //return RedirectToAction("creditorProfile");
            return Json(new { success = true, Message = "Creditor saved successfully.", Creditor = creditor });

        }


        /* Delete creditor */
        [Route("DeleteCreditor")]
        [HttpPost]
        public JsonResult DeleteCreditor(string creditorCode)
        {
            var result = false;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.CreditorBO().DeleteCreditor(new Creditor { CreditorCode = creditorCode });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "creditor deleted successfully.", CreditorCode = creditorCode });

        }


        /* Search Creditor*/
        [Route("SearchCreditor")]
        public ActionResult SearchCreditor(string creditorCode)
        {
            if (creditorCode != "")
            {
                Creditor creditor = null;

                creditor = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = creditorCode });

                if (creditor == null)
                {
                    creditor = new Creditor();
                }

                creditor.CountryList = Utility.GetCountryList();
                creditor.CreditorAccountList = Utility.GetCreditorAccountList();

                return RedirectToAction("CreditorProfile", new { creditorCode = creditorCode });
                
            }
            else
            {
                return RedirectToAction("CreditorProfile");
            }
        }

        #endregion

        #region AP CREDIT NOTE


        [Route("SaveAPCreditNote")]
        [HttpPost]
        public JsonResult SaveAPCreditNote(POSAccount.Contract.APCreditNote apCreditNoteData)
        {
            try
            {
                apCreditNoteData.CreatedBy = Utility.DEFAULTUSER;
                apCreditNoteData.ModifiedBy = Utility.DEFAULTUSER;
                apCreditNoteData.Source = "";


                var result = new POSAccount.BusinessFactory.APCreditNoteBO().SaveAPCreditNote(apCreditNoteData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "Credit Note saved successfully.", apCreditNoteData = apCreditNoteData });


        }


        [Route("DeleteAPCreditNote")]
        [HttpPost]
        public JsonResult DeleteAPCreditNote(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.APCreditNoteBO().DeleteAPCreditNote(new APCreditNote { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Credit Note deleted successfully.", documentNo = documentNo });


        }

        [Route("CreditNote")]
        public ActionResult APCreditNote(string invoiceType, string documentNo)
        {
            APCreditNote apCreditNote = null;
            if (invoiceType == Utility.NEWRECORD)
            {
                apCreditNote = new APCreditNote();
                apCreditNote.DocumentDate = DateTime.UtcNow.ThaiTime();              
                apCreditNote.APCreditNoteDetails = new List<APCreditNoteDetail>();
                apCreditNote.CreditorList = Utility.GetCreditorList();
                apCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();
            }
            else
            {

                if (documentNo != null && documentNo != "")
                {
                    apCreditNote = new POSAccount.BusinessFactory.APCreditNoteBO().GetAPCreditNote(new APCreditNote { DocumentNo = documentNo });
                    apCreditNote.CreditorList = Utility.GetCreditorList();
                    apCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();
                }
                else
                {
                    //apInvoice = new POSAccount.BusinessFactory.APInvoiceBO().GetList().FirstOrDefault();

                    apCreditNote = new APCreditNote();
                    apCreditNote.DocumentDate = DateTime.Today.Date;
                    apCreditNote.APCreditNoteDetails = new List<APCreditNoteDetail>();
                    apCreditNote.CreditorList = Utility.GetCreditorList();
                    apCreditNote.AccountCodeList = Utility.GetAccountCodeItemList();

                }



            }
            //apCreditNote.CreditorList = Utility.GetCreditorList();
            //apCreditNote.CurrencyCodeList = Utility.GetCurrencyItemList();


            return View("APCreditNote", apCreditNote);
        }

        [Route("SearchAPCreditNote")]
        public ActionResult SearchAPCreditNote(string documentNo)
        {
            if (documentNo != "0")
            {
                APCreditNote apCreditNote = null;

                apCreditNote = new POSAccount.BusinessFactory.APCreditNoteBO().GetAPCreditNote(new APCreditNote { DocumentNo = documentNo });

                if (apCreditNote == null)
                {
                    apCreditNote = new APCreditNote();
                    apCreditNote.DocumentDate = DateTime.Today.Date;
                    apCreditNote.APCreditNoteDetails = new List<APCreditNoteDetail>();
                }


                return RedirectToAction("APCreditNote", new { InvoiceType = "", DocumentNo = documentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("APCreditNote");
            }
        }


        [HttpPost]
        public JsonResult AddAPCreditNote(string accountcode, string amount, string customerCode)
        {
            APCreditNoteDetail item = new APCreditNoteDetail();

            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountcode, BranchID = Utility.SsnBranch }).Description;
            item.AccountCode = accountcode;
            item.BaseAmount = Convert.ToDecimal(amount);
            item.LocalAmount = Convert.ToDecimal(amount);
            item.CurrencyCode = "THB";
            item.TaxAmount = 0;
                        
            var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = customerCode }).CreditorAccount;
            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(creditorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            debitCreditObj = Utility.GetAccountType(accountcode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */
            return Json(new
            {
                Message = "Success",
                ARCreditNoteDetail = item,
                CreditorAccount = creditorAccount,
                CustomerDebitCredit = customerDC,
                AccountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);

        }

        [Route("AddAPCreditNoteItem")]
        [HttpGet]
        public ActionResult AddAPCreditNoteItem(string documentNo, Int16 itemNo)
        {
            APCreditNoteDetail apCreditNotedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                apCreditNotedetail = new APCreditNoteDetail();
            }
            else
            {
                apCreditNotedetail = new POSAccount.BusinessFactory.APCreditNoteBO().GetAPCreditNote(new Contract.APCreditNote { DocumentNo = documentNo })
                       .APCreditNoteDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (apCreditNotedetail == null)
                {
                    apCreditNotedetail = new APCreditNoteDetail();
                }
            }

            apCreditNotedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            apCreditNotedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return PartialView("AddAPCreditNoteItem", apCreditNotedetail);
            //return PartialView("AddQuotationItem");
        }
        #endregion

        #region AP DEBIT NOTE


        [Route("SaveAPDebitNote")]
        [HttpPost]
        public JsonResult SaveAPDebitNote(POSAccount.Contract.APDebitNote apDebitNoteData)
        {
            try
            {
                apDebitNoteData.CreatedBy = Utility.DEFAULTUSER;
                apDebitNoteData.ModifiedBy = Utility.DEFAULTUSER;
                apDebitNoteData.Source = "";


                var result = new POSAccount.BusinessFactory.APDebitNoteBO().SaveAPDebitNote(apDebitNoteData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "Debit Note saved successfully.", apDebitNoteData = apDebitNoteData });


        }


        [Route("DeleteAPDebitNote")]
        [HttpPost]
        public JsonResult DeleteAPDebitNote(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.APDebitNoteBO().DeleteAPDebitNote(new APDebitNote { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Debit Note deleted successfully.", documentNo = documentNo });


        }

        [Route("DebitNote")]
        public ActionResult APDebitNote(string invoiceType, string documentNo)
        {
            APDebitNote apDebitNote = null;
            if (invoiceType == Utility.NEWRECORD)
            {
                apDebitNote = new APDebitNote();
                apDebitNote.DocumentDate = DateTime.UtcNow.ThaiTime();
                apDebitNote.APDebitNoteDetails = new List<APDebitNoteDetail>();
                apDebitNote.CreditorList = Utility.GetCreditorList();
                apDebitNote.AccountCodeList = Utility.GetAccountCodeItemList();
            }
            else
            {

                if (documentNo != null && documentNo != "")
                {
                    apDebitNote = new POSAccount.BusinessFactory.APDebitNoteBO().GetAPDebitNote(new APDebitNote { DocumentNo = documentNo });
                    apDebitNote.CreditorList = Utility.GetCreditorList();
                    apDebitNote.AccountCodeList = Utility.GetAccountCodeItemList();
                }
                else
                {
                    //apInvoice = new POSAccount.BusinessFactory.APInvoiceBO().GetList().FirstOrDefault();

                    apDebitNote = new APDebitNote();
                    apDebitNote.DocumentDate = DateTime.UtcNow.ThaiTime();
                    apDebitNote.APDebitNoteDetails = new List<APDebitNoteDetail>();
                    apDebitNote.CreditorList = Utility.GetCreditorList();
                    apDebitNote.AccountCodeList = Utility.GetAccountCodeItemList();
                }
            }
            
            return View("APDebitNote", apDebitNote);
        }

        [Route("SearchAPDebitNote")]
        public ActionResult SearchAPDebitNote(string documentNo)
        {
            if (documentNo != "0")
            {
                APDebitNote apDebitNote = null;

                apDebitNote = new POSAccount.BusinessFactory.APDebitNoteBO().GetAPDebitNote(new APDebitNote { DocumentNo = documentNo });

                if (apDebitNote == null)
                {
                    apDebitNote = new APDebitNote();
                    apDebitNote.DocumentDate = DateTime.Today.Date;
                    apDebitNote.APDebitNoteDetails = new List<APDebitNoteDetail>();
                }


                return RedirectToAction("APDebitNote", new { InvoiceType = "", DocumentNo = documentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("APDebitNote");
            }
        }


        [Route("AddAPDebitNoteItem")]
        [HttpGet]
        public ActionResult AddAPDebitNoteItem(string documentNo, Int16 itemNo)
        {
            APDebitNoteDetail apDebitNotedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                apDebitNotedetail = new APDebitNoteDetail();
            }
            else
            {
                apDebitNotedetail = new POSAccount.BusinessFactory.APDebitNoteBO().GetAPDebitNote(new Contract.APDebitNote { DocumentNo = documentNo })
                       .APDebitNoteDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (apDebitNotedetail == null)
                {
                    apDebitNotedetail = new APDebitNoteDetail();
                }
            }

            apDebitNotedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            apDebitNotedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return PartialView("AddAPDebitNoteItem", apDebitNotedetail);
            //return PartialView("AddQuotationItem");
        }


        [HttpPost]
        public JsonResult AddAPDebitNote(string accountcode, string amount, string customerCode)
        {
            APDebitNoteDetail item = new APDebitNoteDetail();

            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountcode, BranchID = Utility.SsnBranch }).Description;
            item.AccountCode = accountcode;
            item.BaseAmount = Convert.ToDecimal(amount);
            item.LocalAmount = Convert.ToDecimal(amount);
            item.CurrencyCode = "THB";
            item.TaxAmount = 0;

            var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = customerCode }).CreditorAccount;
            /* Debit Credit Account start */
            var debitCreditObj = Utility.GetAccountType(creditorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            debitCreditObj = Utility.GetAccountType(accountcode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */
            return Json(new
            {
                Message = "Success",
                ARCreditNoteDetail = item,
                CreditorAccount = creditorAccount,
                CustomerDebitCredit = customerDC,
                AccountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);

           
            //    var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = customerCode }).CreditorAccount;

            //return Json(new { Message = "Success", APDebitNoteDetail = item, CreditorAccount = creditorAccount}, JsonRequestBehavior.AllowGet);
            
        }

        #endregion

        #region AP INVOICE Action Methods

        [Route("SaveAPInvoice")]
        [HttpPost]
        public JsonResult SaveAPInvoice(POSAccount.Contract.APInvoice apInvoiceData)
        {
            try
            {
                apInvoiceData.CreatedBy = Utility.DEFAULTUSER;
                apInvoiceData.ModifiedBy = Utility.DEFAULTUSER;
                apInvoiceData.Source = "AP";
                apInvoiceData.CurrencyCode = Utility.DEFAULTCURRENCYCODE;
                apInvoiceData.BranchID = Utility.SsnBranch;


                var result = new POSAccount.BusinessFactory.APInvoiceBO().SaveAPInvoice(apInvoiceData);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { success = true, Message = "Invoice saved successfully.", apInvoiceData = apInvoiceData });

            
        }


        [Route("DeleteAPInvoice")]
        [HttpPost]
        public JsonResult DeleteAPInvoice(string documentNo, string cancelBy)
        {
            //CBReceipt CBReceiptdata =
            var result = false;
            cancelBy = Utility.DEFAULTUSER;
            string message = string.Empty;
            try
            {
                result = new POSAccount.BusinessFactory.APInvoiceBO().DeleteAPInvoice(new APInvoice { DocumentNo = documentNo, CancelledBy = cancelBy });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }

            return Json(new { result = result, Message = "Invoice deleted successfully.", documentNo = documentNo });

           
        }

        [Route("Invoice")]
        public ActionResult APInvoice(string invoiceType, string documentNo)
        {
            APInvoice apInvoice = null;
            if (invoiceType == Utility.NEWRECORD)
            {
                apInvoice = new APInvoice();
                apInvoice.DocumentDate = DateTime.UtcNow.ThaiTime();
                apInvoice.APInvoiceDetails = new List<APInvoiceDetail>();
            }
            else
            {

                if (documentNo != null && documentNo != "")
                {
                    apInvoice = new POSAccount.BusinessFactory.APInvoiceBO().GetAPInvoice(new APInvoice { DocumentNo = documentNo });

                }
                else
                {
                    //apInvoice = new POSAccount.BusinessFactory.APInvoiceBO().GetList().FirstOrDefault();
                    
                    apInvoice = new APInvoice();
                    apInvoice.DocumentDate = DateTime.Today.Date;
                    apInvoice.APInvoiceDetails = new List<APInvoiceDetail>();

                  
                }

               

            }

            if (apInvoice!=null)
            {
                apInvoice.CreditorList = Utility.GetCreditorList();
                apInvoice.CurrencyCodeList = Utility.GetCurrencyItemList();
                apInvoice.AccountCodeList = Utility.GetAccountCodeItemList().ToList();
            }


            return View("APInvoice", apInvoice);
        }

        [Route("SearchAPInvoice")]
        public ActionResult SearchAPInvoice(string documentNo)
        {
            if (documentNo != "0")
            {
                APInvoice apinvoice= null;

                apinvoice = new POSAccount.BusinessFactory.APInvoiceBO().GetAPInvoice(new APInvoice { DocumentNo = documentNo });

                if (apinvoice == null)
                {
                    apinvoice = new APInvoice();
                    apinvoice.DocumentDate = DateTime.Today.Date;
                    apinvoice.APInvoiceDetails = new List<APInvoiceDetail>();
                }

                 
                return RedirectToAction("APInvoice", new { InvoiceType = "", DocumentNo = documentNo });
                //return View("CBReceipt", cbReceipt);
            }
            else
            {
                return RedirectToAction("APInvoice");
            }
        }


        [Route("AddAPInvoiceItem")]
        [HttpGet]
        public ActionResult AddAPInvoiceItem(string documentNo, Int16 itemNo)
        {
            APInvoiceDetail apInvoicedetail = null;
            if (documentNo == string.Empty || documentNo == null)
            {
                apInvoicedetail = new APInvoiceDetail();
            }
            else
            {
                apInvoicedetail = new POSAccount.BusinessFactory.APInvoiceBO().GetAPInvoice(new Contract.APInvoice { DocumentNo = documentNo })
                       .APInvoiceDetails.Where(dt => dt.ItemNo == itemNo).FirstOrDefault();
                if (apInvoicedetail == null)
                {
                    apInvoicedetail = new APInvoiceDetail();
                }
            }

            apInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            apInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //arInvoicedetailsItem.ServiceTypeList = Utility.GetLookupItemList("ServiceType");
            return View("AddAPInvoiceItem", apInvoicedetail);
            //return PartialView("AddQuotationItem");
        }


        [Route("AddAPInvoiceItem")]
        [HttpPost]
        public ActionResult AddAPInvoiceItem(string customerCode, string accountCode, decimal amount, string bankCode = "")
        {
            APInvoiceDetail item = new APInvoiceDetail();

            item.AccountCode = accountCode;

            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            item.BaseAmount = amount;
            item.LocalAmount = amount;
            item.CurrencyCode = "THB";
            item.TotalAmount = amount;
            item.Discount = 0;
            item.DiscountType = "NONE";

            var creditorAccount = new POSAccount.BusinessFactory.CreditorBO().GetCreditor(new Creditor { CreditorCode = customerCode }).CreditorAccount;
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
            var debitCreditObj = Utility.GetAccountType(creditorAccount);
            var customerDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            string bankDC = "";
            if (!string.IsNullOrWhiteSpace(bankAccount))
            {
                debitCreditObj = Utility.GetAccountType(bankAccount);
                bankDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            }
            
            debitCreditObj =  Utility.GetAccountType(whAccount);
            string whDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(vatAccount);
            string vatDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";

            debitCreditObj = Utility.GetAccountType(accountCode);
            string accountDC = debitCreditObj != null ? (!string.IsNullOrWhiteSpace(debitCreditObj.DebitCredit) ? debitCreditObj.DebitCredit : "DEBIT") : "DEBIT";
            /* Debit Credit Account end */

            return Json(new {
                Message = "Success",
                APInvoiceDetail = item,
                CreditorAccount = creditorAccount,
                bankAccount = bankAccount,
                whAccount = whAccount,
                vatAccount = vatAccount,
                customerDebitCredit = customerDC,
                bankDebitCredit = bankDC,
                whDebitCredit = whDC,
                vatDebitCredit = vatDC,
                accountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);
        }

        /* The below method is done by kiran on 11-JUN-2016. */
        [Route("AddDAPInvoiceItem")]
        [HttpPost]
        public ActionResult AddDAPInvoiceItem(string customerCode, string accountCode, decimal amount, string bankCode="")
        {
            APInvoiceDetail item = new APInvoiceDetail();

            item.AccountCode = accountCode;

            item.AccountCodeDescription = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch }).Description;
            item.BaseAmount = amount;
            item.LocalAmount = amount;
            item.CurrencyCode = "THB";
            item.TotalAmount = amount;
            item.Discount = 0;
            item.DiscountType = "NONE";

            var debtorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = customerCode }).DebtorAccount;
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
            if (!string.IsNullOrWhiteSpace(bankCode))
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
                APInvoiceDetail = item,
                debtorAccount = debtorAccount,
                bankAccount = bankAccount,
                whAccount = whAccount,
                vatAccount = vatAccount,
                customerDebitCredit = customerDC,
                bankDebitCredit = bankDC,
                whDebitCredit = whDC,
                vatDebitCredit = vatDC,
                accountDebitCredit = accountDC
            }, JsonRequestBehavior.AllowGet);


            //var creditorAccount = new POSAccount.BusinessFactory.DebtorBO().GetDebtor(new Debtor { DebtorCode = customerCode }).DebtorAccount;
            //var bankAccount = new POSAccount.BusinessFactory.BankBO().GetBank(new Bank { BankCode = bankCode }).BankAccount;
            //var whAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARWHACCOUNTCODE);
            //var vatAccount = Utility.GetDefaultAccountCodes(DefaultAccountCodes.ARVATACCOUNTCODE);
            //arInvoicedetail.CurrencyCodeList = Utility.GetCurrencyItemList();
            //arInvoicedetail.AccountCodeList = Utility.GetAccountCodeItemList();

            //return Json(new { Message = "Success", APInvoiceDetail = item, CreditorAccount = creditorAccount, bankAccount = bankAccount, whAccount = whAccount, vatAccount = vatAccount }, JsonRequestBehavior.AllowGet);
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



        [Route("APInvoiceList")]
        [HttpGet]
        public ActionResult APInvoiceList()
        {
            var lstAPInvoices = new POSAccount.BusinessFactory.APInvoiceBO().GetList();
            //HttpContext.Session["OrderItems"] = null;
            return View("APInvoiceList", lstAPInvoices);


        }


        #endregion


        #region EDI AP INVOICE

        [Route("EDIAPInvoiceList")]
        [HttpGet]
        public ActionResult EDIAPInvoiceList()
        {
            var lstEDIAPInvoices = new POSAccount.BusinessFactory.APFileUploadBO().GetList(Utility.SsnBranch);
            //HttpContext.Session["OrderItems"] = null;
            return View("EDIAPInvoiceList", lstEDIAPInvoices);


        }

        [Route("APUploadInvoice")]
        public ActionResult APUploadInvoice( string documentNo)
        {
            POSAccount.Contract.APFileUpload apInvoice = null;
             

                if (documentNo != null && documentNo != "")
                {
                    apInvoice = new POSAccount.BusinessFactory.APFileUploadBO().GetAPFileUpload( new APFileUpload{ DocumentNo = documentNo, BranchID= Utility.SsnBranch });

                }
  

            return View("EDIAPInvoice", apInvoice);
        }


        [Route("SaveEDIAPInvoice")]
        [HttpPost]
        public ActionResult SaveEDIAPInvoice(POSAccount.Contract.APFileUpload obj)
        {
            return View("APInvoiceList");
            
        }

        [Route("addinvoice")]
        [HttpGet]
        public ActionResult addinvoice()
        {
            return View("addinvoice");
        }

        #endregion
    }
}