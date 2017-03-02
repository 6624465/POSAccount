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
    public class ReportParams : IContract
    {
        // Constructor 
        public ReportParams() { }


        public string Year { get; set; }

        public string Period { get; set; }

        public string AccountFrom { get; set; }

        public string AccountTo { get; set; }

        public string PeriodFrom { get; set; }

        public string PeriodTo { get; set; }


        public IEnumerable<SelectListItem> YearList { get; set; }


        public IEnumerable<SelectListItem> PeriodList { get; set; }
        public IEnumerable<SelectListItem> AccountCodeList { get; set; }


    }
}
