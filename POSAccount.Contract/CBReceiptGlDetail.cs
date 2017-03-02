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
    public class CBReceiptGlDetail : IContract
    {
        // Constructor 
        public CBReceiptGlDetail() { }

        // Public Members 

        [DisplayName("DocumentNo")]
        public string DocumentNo { get; set; }

        [DisplayName("ItemNo")]
        public Int32 ItemNo { get; set; }

        [DisplayName("TransactionType")]
        public string TransactionType { get; set; }

        [DisplayName("AccountCode")]
        public string AccountCode { get; set; }

        [DisplayName("BaseAmount")]
        public decimal BaseAmount { get; set; }

        [DisplayName("LocalAmount")]
        public decimal LocalAmount { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("TotalAmount")]
        public decimal TotalAmount { get; set; }


    }
}



