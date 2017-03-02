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
    public class Country : IContract
    {
        // Constructor 
        public Country() { }

        // Public Members 

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string Description { get; set; }




    }
}
