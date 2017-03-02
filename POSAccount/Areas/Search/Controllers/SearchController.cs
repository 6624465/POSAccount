using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using POSAccount.ViewModals.Search;
using POSAccount.Areas.Search.Controllers;
using POSAccount.BusinessFactory;



using System.Data;
using System.Collections;

namespace POSAccount.Areas.Search
{
    public class SearchController : Controller
    {
        /*
        [HttpPost]
        public PartialViewResult VoyageSearch(VoyageSearchWrapper voyageSearchWrapper)
        {
            voyageSearchWrapper.searchVm.whereclause = (voyageSearchWrapper.searchVm.whereclause == null ? "" : voyageSearchWrapper.searchVm.whereclause);
            voyageSearchWrapper.searchVm.txtSearch = (voyageSearchWrapper.searchVm.txtSearch == null ? "" : voyageSearchWrapper.searchVm.txtSearch);
            voyageSearchWrapper.searchVm.SearchBy = (voyageSearchWrapper.searchVm.SearchBy == null ? "" : voyageSearchWrapper.searchVm.SearchBy);

            voyageSearchWrapper.AgentCode = (voyageSearchWrapper.AgentCode == null ? "" : voyageSearchWrapper.AgentCode);
            voyageSearchWrapper.BookingType = (voyageSearchWrapper.BookingType == null ? "" : voyageSearchWrapper.BookingType);
            voyageSearchWrapper.BranchId = (voyageSearchWrapper.BranchId == null ? "" : voyageSearchWrapper.BranchId);
            voyageSearchWrapper.ETA = (voyageSearchWrapper.ETA == null ? "" : voyageSearchWrapper.ETA);
            voyageSearchWrapper.VesselCode = (voyageSearchWrapper.VesselCode == null ? "" : voyageSearchWrapper.VesselCode);

            return PartialView("VesselSchedule", SearchService.VoyageSearch<VesselSchedule>(voyageSearchWrapper));
        }
        */
        [HttpPost]
        public PartialViewResult SearchBox(SearchVm searchVm)
        {
            //searchVm.whereclause = (searchVm.whereclause == null ? "" : searchVm.whereclause);
            //searchVm.txtSearch = (searchVm.txtSearch == null ? "" : searchVm.txtSearch);
            //searchVm.SearchBy = (searchVm.SearchBy == null ? "" : searchVm.SearchBy);

            //var searchResultVm = new SearchResultVm()
            //{
            //    SearchByList = searchcolumnbo.GetCategoryList(searchVm.SearchTable),
            //    ListData = LoadData(searchVm)
            //};
            searchVm.whereclause = (searchVm.whereclause == null ? "" : searchVm.whereclause);
            searchVm.txtSearch = (searchVm.txtSearch == null ? "" : searchVm.txtSearch);
            searchVm.SearchBy = (searchVm.SearchBy == null ? "" : searchVm.SearchBy);

            var strSearchClause = "";
            if (searchVm.txtSearch.Trim().Length > 0)
            {

                if (searchVm.whereclause.Equals("LIKE"))
                    strSearchClause = string.Format("{0} {1} N'%{2}%'", searchVm.SearchBy, searchVm.whereclause, searchVm.txtSearch.Trim());
                else
                    strSearchClause = string.Format("{0} {1} '{2}'", searchVm.SearchBy, searchVm.whereclause, searchVm.txtSearch.Trim());
            }
            ////var searchResultVm = new SearchResultVm()
            ////{
            ////    SearchByList = searchcolumnbo.GetCategoryList(searchVm.SearchTable),
            ////    ListData = LoadData(searchVm)
            ////};


            var searchWrapper = new SearchWrapper();
            searchWrapper.SearchByList = new SearchColumnBO().GetCategoryList(searchVm.SearchTable);
            searchWrapper.searchVm = searchVm;
            //IDataReader searchResult = null;
            //IList IList;
            switch (searchVm.SearchTable)
            {
                case SearchCategories.APINVOICE:
                    var searchResultAPInvoice = new SearchResultVm<APInvoice>();
                    List<APInvoice> lstSearchResultAPInvoice = SearchService.DataReaderMapToList<APInvoice>(new APInvoiceBO().PerformSearch(strSearchClause));
                    searchResultAPInvoice.ListData = lstSearchResultAPInvoice;
                    searchResultAPInvoice.searchWrapper = searchWrapper;
                    return PartialView("_APInvoice", searchResultAPInvoice);
                case SearchCategories.ARINVOICE:
                    var searchResultARInvoice = new SearchResultVm<ARInvoice>();
                    List<ARInvoice> lstSearchResultARInvoice = SearchService.DataReaderMapToList<ARInvoice>(new ARInvoiceBO().PerformSearch(strSearchClause));
                    searchResultARInvoice.ListData = lstSearchResultARInvoice;
                    searchResultARInvoice.searchWrapper = searchWrapper;
                    return PartialView("_ARInvoice", searchResultARInvoice);
                case SearchCategories.CBPAYMENT:
                    var searchResultCBPayment = new SearchResultVm<CBPayment>();
                    List<CBPayment> lstSearchResultCBPayment = SearchService.DataReaderMapToList<CBPayment>(new CBPaymentBO().PerformSearch(strSearchClause));
                    searchResultCBPayment.ListData = lstSearchResultCBPayment;
                    searchResultCBPayment.searchWrapper = searchWrapper;
                    return PartialView("_CBPaymentSearchDialog", searchResultCBPayment);
                case SearchCategories.CBRECEIPT:
                    var searchResultCBReceipt = new SearchResultVm<CBReceipt>();
                    List<CBReceipt> lstSearchResultCBReceipt = SearchService.DataReaderMapToList<CBReceipt>(new CBReceiptBO().PerformSearch(strSearchClause));
                    searchResultCBReceipt.ListData = lstSearchResultCBReceipt;
                    searchResultCBReceipt.searchWrapper = searchWrapper;
                    return PartialView("_CBReceiptSearchDialog", searchResultCBReceipt);
                case SearchCategories.GLJOURNAL:
                    var searchResultGLJournal = new SearchResultVm<GLJournal>();
                    List<GLJournal> lstSearchResultGLJournal = SearchService.DataReaderMapToList<GLJournal>(new GLJournalBO().PerformSearch(strSearchClause));
                    searchResultGLJournal.ListData = lstSearchResultGLJournal;
                    searchResultGLJournal.searchWrapper = searchWrapper;
                    return PartialView("_GLJournalSearchDialog", searchResultGLJournal);
                
                
                
                //case SearchCategories.APINVOICE:
                //    List<APInvoice> customer = SearchService.DataReaderMapToList<APInvoice>(new APInvoiceBO().PerformSearch(""));                   
                    
                //    return PartialView("APInvoice");
                /*
                case SearchCategories.CUSTOMER:
                case SearchCategories.ALLCUSTOMER:
                    return PartialView("Customer", SearchService.Search<Customer>(searchVm));
                case SearchCategories.COUNTRY:
                    return PartialView("Country", SearchService.Search<Country>(searchVm));
                case SearchCategories.CHARGECODE:
                    return PartialView("ChargeCode", SearchService.Search<ChargeCodeMaster>(searchVm));
                
                case SearchCategories.FORWARDER:
                    return PartialView("Forwarder", SearchService.Search<Forwarder>(searchVm));
                case SearchCategories.HAULIER:
                    return PartialView("Haulier", SearchService.Search<Haulier>(searchVm));
                case SearchCategories.CONTAINERMASTER:
                    return PartialView("ContainerMaster", SearchService.Search<ContainerMaster>(searchVm));
                case SearchCategories.WOCONTAINERLIST:
                    return PartialView("WOContainer", SearchService.Search<WOContainer>(searchVm));
                case SearchCategories.TRUCKIN:
                    return PartialView("TruckMovementHd", SearchService.Search<TruckMovementHd>(searchVm));
                case SearchCategories.VESSEL:
                    return PartialView("Vessel", SearchService.Search<Vessel>(searchVm));
                case SearchCategories.PORT:
                case SearchCategories.AREA:
                    return PartialView("PortArea", SearchService.Search<PortArea>(searchVm));
                case SearchCategories.VESSELSCHEDULE:
                    //pending                    
                    return PartialView("VesselSchedule", SearchService.Search<VesselSchedule>(searchVm));
                case SearchCategories.BOOKINGVESSEL:
                    return PartialView("BookingVessel", SearchService.Search<BookingVessel>(searchVm));
                case SearchCategories.ORDER:
                    return PartialView("Order", SearchService.Search<Order>(searchVm));
                case SearchCategories.PENDINGMOVEMENTORDER:
                    return PartialView("PendingMovementOrder", SearchService.Search<PendingMovementOrder>(searchVm));
                case SearchCategories.RETRIEVEIMPORTLCLORDERS:
                    return PartialView("RetrieveImportLCLOrders", SearchService.Search<RetrieveImportLCLOrders>(searchVm));
                case SearchCategories.RETRIEVEEXPORTLCLORDERS:
                    return PartialView("RetrieveExportLCLOrders", SearchService.Search<RetrieveExportLCLOrders>(searchVm));
                case SearchCategories.TRUCKSINYARD:
                    return PartialView("TrucksInYard", SearchService.Search<TrucksInYard>(searchVm));
                case SearchCategories.QUOTATION:
                    return PartialView("Quotation", SearchService.Search<Quotation>(searchVm));
                case SearchCategories.TALLYIN:
                    return PartialView("TallyIn", SearchService.Search<TallyIn>(searchVm));
                case SearchCategories.TALLYOUT:
                    return PartialView("TallyOut", SearchService.Search<TallyOut>(searchVm));
                case SearchCategories.CYDIRECT:
                    return PartialView("CYDirect", SearchService.Search<CYDirect>(searchVm));
                case SearchCategories.TALLYINPENDINGQTY:
                    return PartialView("TallyInPendingQty", SearchService.Search<TallyInPendingQty>(searchVm));
                case SearchCategories.COSTSHEETVASCHARGES:
                    return PartialView("CostSheetVASCharges", SearchService.Search<CostSheetVasCharges>(searchVm));
                case SearchCategories.CASHINVOICE:
                    return PartialView("CashInvoice", SearchService.Search<CashInvoice>(searchVm));
                case SearchCategories.WORKORDERSEARCH:
                    return PartialView("WorkOrder", SearchService.Search<WorkOrder>(searchVm));
                /*
                case SearchCategories.PORTROTATIONVESSELSEARCH:                     
                case SearchCategories.PORTROTATIONVOYAGESEARCH:   */
                default:
                    return PartialView("Order");
            }

        }

    }
}