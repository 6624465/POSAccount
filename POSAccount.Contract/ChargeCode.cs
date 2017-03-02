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
    public class ChargeCodeMaster : IContract
    {
        // Constructor 
        public ChargeCodeMaster() { }

        // Public Members 
        [Required(ErrorMessage = "Charge Code is required.")]
        [DisplayName("Charge Code")]
        public string ChargeCode { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("BranchID")]
        public short BranchID { get; set; }

        [Required(ErrorMessage = "Billing Unit is required.")]
        [DisplayName("Billing Unit")]
        public string BillingUnit { get; set; }

        [Required(ErrorMessage = "Credit Term is required.")]
        [DisplayName("Credit Term")]
        public string CreditTerm { get; set; }

        [Required(ErrorMessage = "Payment Term is required.")]
        [DisplayName("Payment Term")]
        public string PaymentTerm { get; set; }


        [DisplayName("VAT")]
        public decimal VAT { get; set; }

        [DisplayName("VAT Account Code")]
        public string VATAccountCode { get; set; }

        [DisplayName("VATAccount Description")]
        public string VATAccountDescription { get; set; }


        [DisplayName("WithHoldingTax Rate")]
        public decimal WithHoldingTaxRate { get; set; }

        [DisplayName("WH Account Code")]
        public string WHAccountCode { get; set; }

        [DisplayName("WHAccount Description")]
        public string WHAccountDescription { get; set; }

        [Required(ErrorMessage = "Account Code is required.")]
        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

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

        [DisplayName("Account Description")]
        public string AccountDescription { get; set; }

        [DisplayName("Account Description")]
        public string AccountDescription2 { get; set; }


        public IEnumerable<SelectListItem> BillingUnitList { get; set; }

        public IEnumerable<SelectListItem> CreditTermList { get; set; }

        public IEnumerable<SelectListItem> PaymentTermList { get; set; }

        public IEnumerable<SelectListItem> AccountCodeList { get; set; }

        public IEnumerable<SelectListItem> VATAccountCodeList { get; set; }
        public IEnumerable<SelectListItem> WHAccountCodeList { get; set; }

    }
}
