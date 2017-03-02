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
    public class Address : IContract
    {
        // Constructor 
        public Address() { }

        // Public Members 

        public Int64 AddressId { get; set; }

        public string AddressLinkID { get; set; }

        public Int16 SeqNo { get; set; }

        public string AddressType { get; set; }

        [Required(ErrorMessage = "Address1 is required."), StringLength(100)]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        
        public string Address3 { get; set; }
        
        public string Address4 { get; set; }

        public string CityName { get; set; }
        
        public string StateName { get; set; }

        public string CountryCode { get; set; }

        public string ZipCode { get; set; }

        public string TelNo { get; set; }

        public string FaxNo { get; set; }

        public string MobileNo { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public bool IsBilling { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }


    }
}
