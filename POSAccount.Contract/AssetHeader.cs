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
	public class AssetHeader: IContract
	{
		// Constructor 
		public AssetHeader() { }

		// Public Members 

		[DisplayName("AssetCode")] 
		public string AssetCode { get; set; }

		[DisplayName("BranchID")] 
		public Int16  BranchID { get; set; }

		[DisplayName("Description")] 
		public string Description { get; set; }

		[DisplayName("BuyingDate")] 
		public DateTime  BuyingDate { get; set; }

		[DisplayName("Price")] 
		public decimal Price { get; set; }

		[DisplayName("Rate")] 
		public decimal Rate { get; set; }



        [DisplayName("DepreciationType")]
        public string  DepreciationType { get; set; }

		[DisplayName("Status")] 
		public bool  Status { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

        public IEnumerable<SelectListItem> DepreciationTypeList { get; set; }
	}
}



