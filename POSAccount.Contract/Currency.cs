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
    public class Currency : IContract
    {
        // Constructor 
        public Currency() { }

        // Public Members 

        public string CurrencyCode { get; set; }

        public string Description { get; set; }

        public string Description1 { get; set; }


    }
}
