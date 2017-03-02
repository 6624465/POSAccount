using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSAccount.DataFactory
{
    public static class DBRoutine
    {
        /// <summary>
        /// [Master].[Company]
        /// </summary>

        public const string DELETECOMPANY = "[Master].[usp_CompanyDelete]";
        public const string LISTCOMPANY = "[Master].[usp_CompanyList]";
        public const string SAVECOMPANY = "[Master].[usp_CompanySave]";
        public const string SELECTCOMPANY = "[Master].[usp_CompanySelect]";


        /// <summary>
        /// [Master].[Address]
        /// </summary>

        public const string SELECTADDRESS = "[Master].[usp_AddressSelect]";
        public const string LISTADDRESS = "[Master].[usp_AddressList]";
        public const string SAVEADDRESS = "[Master].[usp_AddressSave]";
        public const string DELETEADDRESS = "[Master].[usp_AddressDelete]";
        public const string CONTACTLISTBYCUSTOMER = "[Master].[usp_ContactListByCustomer]";



        /// <summary>
        /// [Master].[Branch]
        /// </summary>

        public const string SELECTBRANCH = "[Master].[usp_BranchSelect]";
        public const string LISTBRANCH = "[Master].[usp_BranchList]";
        public const string SAVEBRANCH = "[Master].[usp_BranchSave]";
        public const string DELETEBRANCH = "[Master].[usp_BranchDelete]";
        public const string LISTBRANCHBYCOMPANY = "[Master].[usp_BranchListByCompany]";


        /// <summary>
        /// [Master].[ChargeCode]
        /// </summary>

        public const string SELECTCHARGECODE = "[Master].[usp_ChargeCodeSelect]";
        public const string LISTCHARGECODE = "[Master].[usp_ChargeCodeList]";
        public const string SAVECHARGECODE = "[Master].[usp_ChargeCodeSave]";
        public const string DELETECHARGECODE = "[Master].[usp_ChargeCodeDelete]";



        /// <summary>
        /// [Master].[Customer]
        /// </summary>

        public const string LISTCONTACTBYCUSTOMER = "[Master].[usp_ContactListByCustomer]";
        public const string SELECTCUSTOMER = "[Master].[usp_CustomerSelect]";
        public const string LISTCUSTOMER = "[Master].[usp_CustomerList]";
        public const string SAVECUSTOMER = "[Master].[usp_CustomerSave]";
        public const string DELETECUSTOMER = "[Master].[usp_CustomerDelete]";



        /// <summary>
        /// [Master].[Country]
        /// </summary>

        public const string SELECTCOUNTRY = "[Master].[usp_CountrySelect]";
        public const string LISTCOUNTRY = "[Master].[usp_CountryList]";
        public const string SAVECOUNTRY = "[Master].[usp_CountrySave]";
        public const string DELETECOUNTRY = "[Master].[usp_CountryDelete]";


        /// <summary>
        /// [Config].[LookUp]
        /// </summary>

        public const string SELECTLOOKUP = "[Config].[usp_LookupSelect]";
        public const string LISTLOOKUP = "[Config].[usp_LookupList]";
        public const string SAVELOOKUP = "[Config].[usp_LookupSave]";
        public const string DELETELOOKUP = "[Config].[usp_LookupDelete]";



        /// <summary>
        /// [Account].[AccountGroup]
        /// </summary>

        public const string SELECTACCOUNTGROUP = "[Account].[usp_AccountGroupSelect]";
        public const string LISTACCOUNTGROUP = "[Account].[usp_AccountGroupList]";
        public const string SAVEACCOUNTGROUP = "[Account].[usp_AccountGroupSave]";
        public const string DELETEACCOUNTGROUP = "[Account].[usp_AccountGroupDelete]";


        /// <summary>
        /// [Account].[ChartOfAccount]
        /// </summary>

        public const string SELECTCHARTOFACCOUNT = "[Account].[usp_ChartOfAccountSelect]";
        public const string LISTCHARTOFACCOUNT = "[Account].[usp_ChartOfAccountList]";
        public const string SAVECHARTOFACCOUNT = "[Account].[usp_ChartOfAccountSave]";
        public const string DELETECHARTOFACCOUNT = "[Account].[usp_ChartOfAccountDelete]";

        public const string LISTCASHBANKACCOUNT = "[Account].[usp_CashBankAccountCodeList]";
        public const string LISTCREDITORACCOUNT = "[Account].[usp_CreditorAccountCodeList]";
        public const string LISTDEBTORACCOUNT = "[Account].[usp_DebtorAccountCodeList]";

        public const string LISTCHARTOFACCOUNTBYACCOUNTGROUP = "[Account].[usp_ChartOfAccountListByAccountGroup]";
        




        /// <summary>
        /// [Master].[Currency]
        /// </summary>

        public const string SELECTCURRENCY = "[Master].[usp_CurrencySelect]";
        public const string LISTCURRENCY = "[Master].[usp_CurrencyList]";
        public const string SAVECURRENCY = "[Master].[usp_CurrencySave]";
        public const string DELETECURRENCY = "[Master].[usp_CurrencyDelete]";



        /// <summary>
        /// [Security].[Users]
        /// </summary>

        public const string SELECTUSERS = "[Security].[usp_UsersSelect]";
        public const string LISTUSERS = "[Security].[usp_UsersList]";
        public const string SAVEUSERS = "[Security].[usp_UsersSave]";
        public const string DELETEUSERS = "[Security].[usp_UsersDelete]";


        /// <summary>
        /// [Master].[Quotation]
        /// </summary>

        public const string LISTQUOTATION = "[Master].[usp_QuotationList]";
        public const string SAVEQUOTATION = "[Master].[usp_QuotationSave]";
        public const string DELETEQUOTATION = "[Master].[usp_QuotationDelete]";
        public const string SELECTQUOTATION = "[Master].[usp_QuotationSelect]";


        /// <summary>
        /// [Master].[QuotationItem]
        /// </summary>

        public const string SELECTQUOTATIONITEM = "[Master].[usp_QuotationItemSelect]";
        public const string LISTQUOTATIONITEM = "[Master].[usp_QuotationItemList]";
        public const string SAVEQUOTATIONITEM = "[Master].[usp_QuotationItemSave]";
        public const string DELETEQUOTATIONITEM = "[Master].[usp_QuotationItemDelete]";

        /// <summary>
        /// [Account].[Creditor]
        /// </summary>

        public const string SELECTCREDITOR = "[Account].[usp_CreditorSelect]";
        public const string LISTCREDITOR = "[Account].[usp_CreditorList]";
        public const string SAVECREDITOR = "[Account].[usp_CreditorSave]";
        public const string DELETECREDITOR = "[Account].[usp_CreditorDelete]";


        /// <summary>
        /// [Account].[Debtor]
        /// </summary>

        public const string SELECTDEBTOR = "[Account].[usp_DebtorSelect]";
        public const string LISTDEBTOR = "[Account].[usp_DebtorList]";
        public const string SAVEDEBTOR = "[Account].[usp_DebtorSave]";
        public const string DELETEDEBTOR = "[Account].[usp_DebtorDelete]";
        public const string LISTDEBTORAUTOSEARCH = "[Account].[usp_DebtorListAutoSearch]";
        

        /// <summary>
        /// [Operation].[OrderHeader]
        /// </summary>

        public const string SELECTORDERHEADER = "[Operation].[usp_OrderHeaderSelect]";
        public const string LISTORDERHEADER = "[Operation].[usp_OrderHeaderList]";
        public const string SAVEORDERHEADER = "[Operation].[usp_OrderHeaderSave]";
        public const string DELETEORDERHEADER = "[Operation].[usp_OrderHeaderDelete]";

        

        /// <summary>
        /// [Operation].[OrderDetail]
        /// </summary>

        public const string SELECTORDERDETAILS = "[Operation].[usp_OrderDetailsSelect]";
        public const string LISTORDERDETAILS = "[Operation].[usp_OrderDetailsList]";
        public const string SAVEORDERDETAILS = "[Operation].[usp_OrderDetailsSave]";
        public const string DELETEORDERDETAILS = "[Operation].[usp_OrderDetailsDelete]";

        public const string GETCHARGECODEPRICE = "[Master].[usp_GetChargeCodePrice]";

        /// <summary>
        /// [Account].[ARInvoiceDetail]
        /// </summary>

        public const string SELECTARINVOICEDETAIL = "[Account].[usp_ARInvoiceDetailSelect]";
        public const string LISTARINVOICEDETAIL = "[Account].[usp_ARInvoiceDetailList]";
        public const string SAVEARINVOICEDETAIL = "[Account].[usp_ARInvoiceDetailSave]";
        public const string DELETEARINVOICEDETAIL = "[Account].[usp_ARInvoiceDetailDelete]";

        /// <summary>
        /// [Account].[ARInvoice]
        /// </summary>


        public const string SELECTARINVOICE = "[Account].[usp_ARInvoiceSelect]";
        public const string LISTARINVOICE = "[Account].[usp_ARInvoiceList]";
        public const string SAVEARINVOICE = "[Account].[usp_ARInvoiceSave]";
        public const string DELETEARINVOICE = "[Account].[usp_ARInvoiceDelete]";


        /// <summary>
        /// [Account].[APInvoiceDetail]
        /// </summary>

        public const string SELECTAPINVOICEDETAIL = "[Account].[usp_APInvoiceDetailSelect]";
        public const string LISTAPINVOICEDETAIL = "[Account].[usp_APInvoiceDetailList]";
        public const string SAVEAPINVOICEDETAIL = "[Account].[usp_APInvoiceDetailSave]";
        public const string DELETEAPINVOICEDETAIL = "[Account].[usp_APInvoiceDetailDelete]";

        /// <summary>
        /// [Account].[APInvoice]
        /// </summary>


        public const string SELECTAPINVOICE = "[Account].[usp_APInvoiceSelect]";
        public const string LISTAPINVOICE = "[Account].[usp_APInvoiceList]";
        public const string SAVEAPINVOICE = "[Account].[usp_APInvoiceSave]";
        public const string DELETEAPINVOICE = "[Account].[usp_APInvoiceDelete]";



        /// <summary>
        /// [Account].[ARCreditNote]
        /// </summary>

        public const string SELECTARCREDITNOTE = "[Account].[usp_ARCreditNoteSelect]";
        public const string LISTARCREDITNOTE = "[Account].[usp_ARCreditNoteList]";
        public const string SAVEARCREDITNOTE = "[Account].[usp_ARCreditNoteSave]";
        public const string DELETEARCREDITNOTE = "[Account].[usp_ARCreditNoteDelete]";


        /// <summary>
        /// [Account].[ARCreditNoteDetail]
        /// </summary>
        public const string SELECTARCREDITNOTEDETAIL = "[Account].[usp_ARCreditNoteDetailSelect]";
        public const string LISTARCREDITNOTEDETAIL = "[Account].[usp_ARCreditNoteDetailList]";
        public const string SAVEARCREDITNOTEDETAIL = "[Account].[usp_ARCreditNoteDetailSave]";
        public const string DELETEARCREDITNOTEDETAIL = "[Account].[usp_ARCreditNoteDetailDelete]";

        /// <summary>
        /// [Account].[ARDebitNote]
        /// </summary>

        public const string SELECTARDEBITNOTE = "[Account].[usp_ARDebitNoteSelect]";
        public const string LISTARDEBITNOTE = "[Account].[usp_ARDebitNoteList]";
        public const string SAVEARDEBITNOTE = "[Account].[usp_ARDebitNoteSave]";
        public const string DELETEARDEBITNOTE = "[Account].[usp_ARDebitNoteDelete]";


        /// <summary>
        /// [Account].[ARDebitNoteDetail]
        /// </summary>
        public const string SELECTARDEBITNOTEDETAIL = "[Account].[usp_ARDebitNoteDetailSelect]";
        public const string LISTARDEBITNOTEDETAIL = "[Account].[usp_ARDebitNoteDetailList]";
        public const string SAVEARDEBITNOTEDETAIL = "[Account].[usp_ARDebitNoteDetailSave]";
        public const string DELETEARDEBITNOTEDETAIL = "[Account].[usp_ARDebitNoteDetailDelete]";




        /// <summary>
        /// [Account].[APCreditNote]
        /// </summary>

        public const string SELECTAPCREDITNOTE = "[Account].[usp_APCreditNoteSelect]";
        public const string LISTAPCREDITNOTE = "[Account].[usp_APCreditNoteList]";
        public const string SAVEAPCREDITNOTE = "[Account].[usp_APCreditNoteSave]";
        public const string DELETEAPCREDITNOTE = "[Account].[usp_APCreditNoteDelete]";


        /// <summary>
        /// [Account].[APCreditNoteDetail]
        /// </summary>
        public const string SELECTAPCREDITNOTEDETAIL = "[Account].[usp_APCreditNoteDetailSelect]";
        public const string LISTAPCREDITNOTEDETAIL = "[Account].[usp_APCreditNoteDetailList]";
        public const string SAVEAPCREDITNOTEDETAIL = "[Account].[usp_APCreditNoteDetailSave]";
        public const string DELETEAPCREDITNOTEDETAIL = "[Account].[usp_APCreditNoteDetailDelete]";




        /// <summary>
        /// [Account].[APDebitNote]
        /// </summary>
        public const string SELECTAPDEBITNOTEDETAIL = "[Account].[usp_APDebitNoteDetailSelect]";
        public const string LISTAPDEBITNOTEDETAIL = "[Account].[usp_APDebitNoteDetailList]";
        public const string SAVEAPDEBITNOTEDETAIL = "[Account].[usp_APDebitNoteDetailSave]";
        public const string DELETEAPDEBITNOTEDETAIL = "[Account].[usp_APDebitNoteDetailDelete]";


        /// <summary>
        /// [Account].[APDebitNoteDetail]
        /// </summary>
        public const string SELECTAPDEBITNOTE = "[Account].[usp_APDebitNoteSelect]";
        public const string LISTAPDEBITNOTE = "[Account].[usp_APDebitNoteList]";
        public const string SAVEAPDEBITNOTE = "[Account].[usp_APDebitNoteSave]";
        public const string DELETEAPDEBITNOTE = "[Account].[usp_APDebitNoteDelete]";

        /// <summary>
        /// [Account].[Bank]
        /// </summary>

        public const string SELECTBANK = "[Account].[usp_BankSelect]";
        public const string LISTBANK = "[Account].[usp_BankList]";
        public const string SAVEBANK = "[Account].[usp_BankSave]";
        public const string DELETEBANK = "[Account].[usp_BankDelete]";

        /// <summary>
        /// [Account].[CBPaymentDetail]
        /// </summary>

        public const string SELECTCBPAYMENTDETAIL = "[Account].[usp_CBPaymentDetailSelect]";
        public const string LISTCBPAYMENTDETAIL = "[Account].[usp_CBPaymentDetailList]";
        public const string SAVECBPAYMENTDETAIL = "[Account].[usp_CBPaymentDetailSave]";
        public const string DELETECBPAYMENTDETAIL = "[Account].[usp_CBPaymentDetailDelete]";

        /// <summary>
        /// [Account].[CBPayment]
        /// </summary>

        public const string SELECTCBPAYMENT = "[Account].[usp_CBPaymentSelect]";
        public const string LISTCBPAYMENT = "[Account].[usp_CBPaymentList]";
        public const string SAVECBPAYMENT = "[Account].[usp_CBPaymentSave]";
        public const string DELETECBPAYMENT = "[Account].[usp_CBPaymentDelete]";


        /// <summary>
        /// [Account].[CBPaymentSetOffDetail]
        /// </summary>

        public const string SELECTCBPAYMENTSETOFFDETAIL = "[Account].[usp_CBPaymentSetOffDetailSelect]";
        public const string LISTCBPAYMENTSETOFFDETAIL = "[Account].[usp_CBPaymentSetOffDetailList]";
        public const string SAVECBPAYMENTSETOFFDETAIL = "[Account].[usp_CBPaymentSetOffDetailSave]";
        public const string DELETECBPAYMENTSETOFFDETAIL = "[Account].[usp_CBPaymentSetOffDetailDelete]";

        public const string GETCREDITOROUTSTANDINGDOCUMENTS = "[Account].[usp_GetCreditorOutStandingDocuments]";


        /// <summary>
        /// [Account].[CBPaymentGlDetail]
        /// </summary>

        public const string SELECTCBPAYMENTGLDETAIL = "[Account].[usp_CBPaymentGlDetailSelect]";
        public const string LISTCBPAYMENTGLDETAIL = "[Account].[usp_CBPaymentGlDetailList]";
        public const string SAVECBPAYMENTGLDETAIL = "[Account].[usp_CBPaymentGlDetailSave]";
        public const string DELETECBPAYMENTGLDETAIL = "[Account].[usp_CBPaymentGlDetailDelete]";

        /// <summary>
        /// [Account].[CBReceipt]
        /// </summary>

        public const string SELECTCBRECEIPT = "[Account].[usp_CBReceiptSelect]";
        public const string LISTCBRECEIPT = "[Account].[usp_CBReceiptList]";
        public const string SAVECBRECEIPT = "[Account].[usp_CBReceiptSave]";
        public const string DELETECBRECEIPT = "[Account].[usp_CBReceiptDelete]";

        /// <summary>
        /// [Account].[CBReceiptDetail]
        /// </summary>
        public const string SELECTCBRECEIPTDETAIL = "[Account].[usp_CBReceiptDetailSelect]";
        public const string LISTCBRECEIPTDETAIL = "[Account].[usp_CBReceiptDetailList]";
        public const string SAVECBRECEIPTDETAIL = "[Account].[usp_CBReceiptDetailSave]";
        public const string DELETECBRECEIPTDETAIL = "[Account].[usp_CBReceiptDetailDelete]";

        /// <summary>
        /// [Account].[CBReceiptSetOffDetail]
        /// </summary>

        public const string SELECTCBRECEIPTSETOFFDETAIL = "[Account].[usp_CBReceiptSetOffDetailSelect]";
        public const string LISTCBRECEIPTSETOFFDETAIL = "[Account].[usp_CBReceiptSetOffDetailList]";
        public const string SAVECBRECEIPTSETOFFDETAIL = "[Account].[usp_CBReceiptSetOffDetailSave]";
        public const string DELETECBRECEIPTSETOFFDETAIL = "[Account].[usp_CBReceiptSetOffDetailDelete]";

        public const string GETDEBTOROUTSTANDINGDOCUMENTS = "[Account].[usp_GetDebtorOutStandingDocuments]";

        /// <summary>
        /// [Account].[CBReceiptGlDetail]
        /// </summary>

        public const string SELECTCBRECEIPTGLDETAIL = "[Account].[usp_CBReceiptGlDetailSelect]";
        public const string LISTCBRECEIPTGLDETAIL = "[Account].[usp_CBReceiptGlDetailList]";
        public const string SAVECBRECEIPTGLDETAIL = "[Account].[usp_CBReceiptGlDetailSave]";
        public const string DELETECBRECEIPTGLDETAIL = "[Account].[usp_CBReceiptGlDetailDelete]";



        /// <summary>
        /// [Account].[CBBankTransfer]
        /// </summary>

        public const string SELECTCBBANKTRANSFER = "[Account].[usp_CBBankTransferSelect]";
        public const string LISTCBBANKTRANSFER = "[Account].[usp_CBBankTransferList]";
        public const string SAVECBBANKTRANSFER = "[Account].[usp_CBBankTransferSave]";
        public const string DELETECBBANKTRANSFER = "[Account].[usp_CBBankTransferDelete]";

        /// <summary>
        /// [Account].[GLJournalDetail]
        /// </summary>

        public const string SAVEGLJOURNALDETAIL = "[Account].[usp_GLJournalDetailSave]";
        public const string DELETEGLJOURNALDETAIL = "[Account].[usp_GLJournalDetailDelete]";
        public const string SELECTGLJOURNALDETAIL = "[Account].[usp_GLJournalDetailSelect]";
        public const string LISTGLJOURNALDETAIL = "[Account].[usp_GLJournalDetailList]";


        /// <summary>
        /// [Account].[GLJournal]
        /// </summary>

        public const string SELECTGLJOURNAL = "[Account].[usp_GLJournalSelect]";
        public const string LISTGLJOURNAL = "[Account].[usp_GLJournalList]";
        public const string SAVEGLJOURNAL = "[Account].[usp_GLJournalSave]";
        public const string DELETEGLJOURNAL = "[Account].[usp_GLJournalDelete]";



        /// <summary>
        /// [Account].[GLTransaction]
        /// </summary>

        public const string SELECTGLTRANSACTION = "[Account].[usp_GLTransactionSelect]";
        public const string LISTGLTRANSACTION = "[Account].[usp_GLTransactionList]";
        public const string SAVEGLTRANSACTION = "[Account].[usp_GLTransactionSave]";
        public const string DELETEGLTRANSACTION = "[Account].[usp_GLTransactionDelete]";
        public const string DELETEGLTRANSACTIONBYDOCUMENTNO = "[Account].[usp_GLTransactionDeleteByDocumentNo]";
        public const string LISTBANKRECONCILIATIONTRANSACTION = "[Account].[usp_BankReconciliationList]";

        /** Search Functions **/


        public const string SEARCHCREDITOR = "[Account].[usp_PerformCreditorSearch]";
        public const string SEARCHDEBTOR = "[Account].[usp_PerformDebtorSearch]";
        public const string SEARCHORDER = "[Operation].[usp_PerformOrderSearch]";
        public const string SEARCHAPINVOICE  = "[Account].[usp_PerformAPInvoiceSearch]";
        public const string SEARCHARINVOICE = "[Account].[usp_PerformARInvoiceSearch]";
        public const string SEARCHRECEIPT = "[Account].[usp_PerformCBReceiptSearch]";
        public const string SEARCHPAYMENT = "[Account].[usp_PerformCBPaymentSearch]";
        public const string SEARCHGL = "[Account].[usp_PerformGLSearch]";



        /// <summary>
        /// [Utility].[SearchColumn]
        /// </summary>

        public const string LISTSEARCHCOLUMN = "[Utility].[usp_SearchColumnList]";
        public const string LISTSEARCHCOLUMNBYCATEGORY = "[Utility].[usp_SearchColumnListByCategory]";
        public const string SAVESEARCHCOLUMN = "[Utility].[usp_SearchColumnSave]";
        public const string SELECTSEARCHCOLUMN = "[Utility].[usp_SearchColumnSelect]";


        /// <summary>
        /// [Account].[GLFinancialPeriod]
        /// </summary>

        public const string SELECTGLFINANCIALPERIOD = "[Account].[usp_GLFinancialPeriodSelect]";
        public const string LISTGLFINANCIALPERIOD = "[Account].[usp_GLFinancialPeriodList]";
        public const string SAVEGLFINANCIALPERIOD = "[Account].[usp_GLFinancialPeriodSave]";
        public const string DELETEGLFINANCIALPERIOD = "[Account].[usp_GLFinancialPeriodDelete]";


        /// <summary>
        /// [Account].[GLOpeningDetail]
        /// </summary>

        public const string SELECTASSETHEADER = "[Master].[usp_AssetHeaderSelect]";
        public const string LISTASSETHEADER = "[Master].[usp_AssetHeaderList]";
        public const string SAVEASSETHEADER = "[Master].[usp_AssetHeaderSave]";
        public const string DELETEASSETHEADER = "[Master].[usp_AssetHeaderDelete]";


        /// <summary>
        /// [Account].[GLOpening]
        /// </summary>
        public const string SELECTGLOPENINGDETAIL = "[Account].[usp_GLOpeningDetailSelect]";
        public const string LISTGLOPENINGDETAIL = "[Account].[usp_GLOpeningDetailList]";
        public const string SAVEGLOPENINGDETAIL = "[Account].[usp_GLOpeningDetailSave]";
        public const string DELETEGLOPENINGDETAIL = "[Account].[usp_GLOpeningDetailDelete]";


        /// <summary>
        /// [Master].[AssetHeader]
        /// </summary>

        public const string SELECTGLOPENING = "[Account].[usp_GLOpeningSelect]";
        public const string LISTGLOPENING = "[Account].[usp_GLOpeningList]";
        public const string SAVEGLOPENING = "[Account].[usp_GLOpeningSave]";
        public const string DELETEGLOPENING = "[Account].[usp_GLOpeningDelete]";


        /// <summary>
        /// [EDI].[APFileUpload]
        /// </summary>

        public const string SELECTAPFILEUPLOAD = "[EDI].[usp_APFileUploadSelect]";
        public const string LISTAPFILEUPLOAD = "[EDI].[usp_APFileUploadList]";
        public const string SAVEAPFILEUPLOAD = "[EDI].[usp_APFileUploadSave]";
        public const string DELETEAPFILEUPLOAD = "[EDI].[usp_APFileUploadDelete]";


        /// <summary>
        /// [EDI].[APFileDetail]
        /// </summary>

        public const string SELECTAPFILEDETAIL = "[EDI].[usp_APFileDetailSelect]";
        public const string LISTAPFILEDETAIL = "[EDI].[usp_APFileDetailList]";
        public const string SAVEAPFILEDETAIL = "[EDI].[usp_APFileDetailSave]";
        public const string DELETEAPFILEDETAIL = "[EDI].[usp_APFileDetailDelete]";


        /// <summary>
        /// [Security].[RoleRights]
        /// </summary>

        public const string SELECTROLERIGHTS = "[Security].[usp_RoleRightsSelect]";
        public const string LISTROLERIGHTS = "[Security].[usp_RoleRightsList]";
        public const string SAVEROLERIGHTS = "[Security].[usp_RoleRightsSave]";
        public const string DELETEROLERIGHTS = "[Security].[usp_RoleRightsDelete]";

        public const string LISTROLERIGHTSBYROLE = "[Security].[usp_RoleRightsListByRole]";


        public const string LISTSECURABLES = "[Security].[usp_SecurablesList]";

        public const string DELETEALLRIGHTSOFROLE = "[Security].[usp_DeleteRightsOfRole]"; 
        
        /// <summary>
        /// [Security].[Roles]
        /// </summary>

        public const string SELECTROLES = "[Security].[usp_RolesSelect]";
        public const string LISTROLES = "[Security].[usp_RolesList]";
        public const string SAVEROLES = "[Security].[usp_RolesSave]";
        public const string DELETEROLES = "[Security].[usp_RolesDelete]";


        /// <summary>
        /// [Config].[SystemWideConfiguration]
        /// </summary>

        public const string SELECTSYSTEMWIDECONFIGURATION = "[Config].[usp_SystemWideConfigurationSelect]";
        public const string LISTSYSTEMWIDECONFIGURATION = "[Config].[usp_SystemWideConfigurationList]";
        public const string DELETESYSTEMWIDECONFIGURATION = "[Config].[usp_SystemWideConfigurationDelete]";
        public const string SAVESYSTEMWIDECONFIGURATION = "[Config].[usp_SystemWideConfigurationSave]";


    }
}
