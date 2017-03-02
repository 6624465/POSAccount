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
    public class Lookup : IContract
    {
        // Constructor 
        public Lookup() { }

        // Public Members 

        public string LookupCode { get; set; }

        public string Description { get; set; }

        public string Description2 { get; set; }

        public string Category { get; set; }

        public bool Status { get; set; }


    }
}
