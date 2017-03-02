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

    public class ChartOfAccount : IContract
    {
        // Constructor 
        public ChartOfAccount()
        {


        }

        // Public Members 

         
        [DisplayName("Account Code")]
        public string AccountCode { get; set; }

         
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Description 2")]
        public string Description2 { get; set; }

        
        [DisplayName("Account Group")]
        public string AccountGroup { get; set; }

        
        [DisplayName("Debit/Credit")]
        public string DebitCredit { get; set; }

        [DisplayName("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Sequence")]
        public Int16 Sequence { get; set; }


        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        public DateTime ModifiedOn { get; set; }

        public string AccountGroupDescription { get; set; }

        public IEnumerable<SelectListItem> AccountGroupList { get; set; }

        public IEnumerable<SelectListItem> DebitCreditList { get; set; }

        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }


    }
}
