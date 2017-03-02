using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.ValidationAttribute;
using System.Web;
using POSAccount.Contract;

namespace POSAccount.Contract
{
    public class Creditor : IContract
    {
        // Constructor 
        public Creditor() { }

        // Public Members 

         [Required(ErrorMessage = "Supplier Code is required.")]
        [DisplayName("CreditorCode")]
        public string CreditorCode { get; set; }

         [Required(ErrorMessage = "Supplier Name is required.")]
        [DisplayName("CreditorName")]
        public string CreditorName { get; set; }

        [DisplayName("RegistrationNo")]
        [Required(ErrorMessage = "Registration No. is required.")]
        public string RegistrationNo { get; set; }

        [DisplayName("VATNo")]
        public string VATNo { get; set; }

        [DisplayName("CreditTerm")]
        [Required(ErrorMessage = "Credit Term is required.")]
        public string CreditTerm { get; set; }

        [DisplayName("CreditorAccount")]
        [Required(ErrorMessage = "Creditor Account is required.")]
        public string CreditorAccount { get; set; }

        [DisplayName("AddressID")]
        public string AddressID { get; set; }

        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [DisplayName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("IsAutoSendInvoice")]
        public bool IsAutoSendInvoice { get; set; }


        [DisplayName("Payment Type")]
        public string PaymentType { get; set; }

        [DisplayName("CreditorAccount Description")]
        public string CreditorAccountDescription { get; set; }

        public Address CreditorAddress { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public IEnumerable<SelectListItem> CreditorAccountList { get; set; }
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

        


    }
}



