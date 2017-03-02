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
    public class Branch : IContract
    {
        // Constructor 
        public Branch() { }

        // Public Members 

        public Int16 BranchID { get; set; }

        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public string RegNo { get; set; }

        public bool IsActive { get; set; }

        public string CompanyCode { get; set; }

        public string AddressID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Address BranchAddress { get; set; }


    }
}
