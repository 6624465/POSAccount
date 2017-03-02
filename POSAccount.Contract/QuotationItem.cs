using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using POSAccount.Contract;

namespace POSAccount.Contract
{
    public class QuotationItem : IContract
    {
        // Constructor 
        public QuotationItem() { }

        // Public Members 

        [DisplayName("Quotation No.")]
        public string QuotationNo { get; set; }

        [DisplayName("ItemID")]
        public Int16 ItemID { get; set; }


        [DisplayName("Charge Code")]
        public string ChargeCode { get; set; }

        
        [DisplayName("Charge Code Descrption")]
        public string ChargeCodeDescription { get; set; }


        [DisplayName("Sell Rate")]
        public decimal SellRate { get; set; }

        [DisplayName("Slab Rate")]
        public bool SlabRate { get; set; }

        [DisplayName("SlabRate From")]
        public Int16 SlabRateFrom { get; set; }

        [DisplayName("SlabRate To")]
        public Int16 SlabRateTo { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        public DateTime ModifiedOn { get; set; }


        public IEnumerable<SelectListItem> ChargeCodeList { get; set; }


    }
}
