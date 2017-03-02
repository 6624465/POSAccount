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
    public class Bank : IContract
    {
        // Constructor 
        public Bank() { }

        // Public Members 
        [Required(ErrorMessage = "BankCode is required.")]
        [DisplayName("BankCode")]
        public string BankCode { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "AccountNo is required.")]
        [DisplayName("AccountNo")]
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "CurrencyCode is required.")]
        [DisplayName("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessage = "BankAccount is required.")]
        [DisplayName("BankAccount")]
        public string BankAccount { get; set; }

        [DisplayName("OverdraftLimit")]
        public decimal OverdraftLimit { get; set; }

        [DisplayName("CurrentBalance")]
        public decimal CurrentBalance { get; set; }

        [DisplayName("SwiftCode")]
        public string SwiftCode { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("AddressId")]
        public string AddressId { get; set; }

        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [DisplayName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        public Address BankAddress { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public IEnumerable<SelectListItem> BankAccountList { get; set; }


    }
}



