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

    public class Company : IContract
    {
        // Constructor 
        public Company() { }

        // Public Members 

        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string RegNo { get; set; }

        public object Logo { get; set; }

        public bool IsActive { get; set; }

        public string AddressID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Address CompanyAddress { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public List<Branch> BranchList { get; set; }

    }
}
