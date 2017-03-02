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

    public class Customer : IContract
    {
        // Constructor 
        public Customer() { }

        // Public Members 

        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Registration No.")]
        public string RegistrationNo { get; set; }

        [DisplayName("Creditor Code")]
        public string CreditorCode { get; set; }

        [DisplayName("Debtor Code")]
        public string DebtorCode { get; set; }

        [DisplayName("Revenue Account")]
        public string RevenueAccount { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("Credit Term")]
        public Int16 CreditTerm { get; set; }

        [DisplayName("AddressID")]
        public string AddressID { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Cancelled By")]
        public string CancelledBy { get; set; }

        [DisplayName("Cancelled On")]
        public DateTime CancelledOn { get; set; }

        [DisplayName("Customer Type")]
        public string CustomerType { get; set; }



        public Address CustomerAddress { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> CustomerTypeList { get; set; }
    }
}
