using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using POSAccount.Contract;

namespace POSAccount.ViewModals.Search
{
    public class SearchVm
    {
        public string SearchTable { get; set; }
        public string whereclause { get; set; }
        public string txtSearch { get; set; }
        public string SearchBy { get; set; }
    }

    public class VoyageSearchWrapper
    {
        

        public string AgentCode { get; set; }
        public string BookingType { get; set; }
        public string BranchId { get; set; }
        public string ETA { get; set; }
        public SearchVm searchVm { get; set; }
        public string VesselCode { get; set; }
    }

    public static class SearchCategories
    {
        public const string ORDER = "Agent";
        public const string APINVOICE = "APInvoice";
        public const string ARINVOICE = "ARInvoice";
        public const string CBPAYMENT = "CBPayment";
        public const string CBRECEIPT = "CBReceipt";
        public const string GLJOURNAL = "GLJournal";


        /*
        public const string AREA = "Area";
        public const string BOOKINGVESSEL = "BookingVessel";
        public const string CASHINVOICE = "CashInvoice";
        public const string CHARGECODE = "ChargeCode";
        public const string CONTAINERMASTER = "ContainerMaster";
        public const string COSTSHEETVASCHARGES = "CostSheetVASCharges";
        public const string COUNTRY = "Country";
        public const string CUSTOMER = "Customer";
        public const string CYDIRECT = "CYDirect";
        public const string FORWARDER = "Forwarder";
        public const string HAULIER = "Haulier";
        public const string ORDER = "Booking";
        public const string OWNER = "Owner";
        public const string PENDINGMOVEMENTORDER = "PendingMovementOrder";
        public const string PORT = "Port";
        public const string PORTROTATIONVESSELSEARCH = "PortRotationVesselSearch";
        public const string PORTROTATIONVOYAGESEARCH = "PortRotationVoyageSearch";
        public const string QUOTATION = "Quotation";
        public const string RETRIEVEEXPORTLCLORDERS = "RetrieveEXPORTLCLOrders";
        public const string RETRIEVEIMPORTLCLORDERS = "RetrieveIMPORTLCLOrders";
        public const string TALLYIN = "TallyIn";
        public const string TALLYINPENDINGQTY = "TallyInPendingQty";
        public const string TALLYOUT = "TallyOut";
        public const string TRUCKIN = "TruckIn";
        public const string TRUCKSINYARD = "TrucksInYard";
        public const string VESSEL = "Vessel";
        public const string VESSELSCHEDULE = "VesselSchedule";
        public const string WOCONTAINERLIST = "WOContainerList";
        public const string WORKORDERSEARCH = "WorkOrder";
         */
    }

    public class SearchResultVm<T>
    {
        public List<T> ListData { get; set; }
        public SearchWrapper searchWrapper { get; set; }
    }

    public class SearchWrapper
    {
        public List<SearchColumn> SearchByList { get; set; }
        public SearchVm searchVm { get; set; }

        public SearchWrapper()
        {
            this.SearchByList = new List<SearchColumn>();
        }
    }

   

    public class APInvoice
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string CreditorCode { get; set; }
    }


    public class ARInvoice
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string DebtorCode { get; set; }
    }

    public class CBPayment
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string CreditorCode { get; set; }
    }

    public class CBReceipt
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string DebtorCode { get; set; }
    }

    public class GLJournal
    {
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        
    }
}