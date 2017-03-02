using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSAccount;
using POSAccount.Contract;
using POSAccount.BusinessFactory;

namespace POSAccount.Areas.MasterData.Controllers
{
    [RouteArea("MasterData")]
    [SessionFilter]
    public class MasterDataController : Controller
    {
        // GET: MasterData/MasterData
        public ActionResult Index()
        {
            return View();
        }
        /*
        [ChildActionOnly]
        [Route("GetCompanies")]
        public PartialViewResult GetCompanies()
        {
            var companyList = new POSAccount.BusinessFactory.CompanyBO().GetList();
            return PartialView(companyList);
        }

        [Route("Company")]
        public ActionResult CompanyProfile(string companyCode)
        {
            var companyprofile = default(Company);
            if (companyCode == Utility.NEWRECORD)
            {
                companyprofile = new Company();
            }
            else
            {
                companyprofile = new POSAccount.BusinessFactory.CompanyBO().GetList().FirstOrDefault();
            }
            if (companyprofile != null)
            {
                var companyBO = new POSAccount.BusinessFactory.CompanyBO();
                companyprofile.CompanyAddress = companyBO.GetCompanyAddress(companyprofile);
                companyprofile.CountryList = Utility.GetCountryList();
            }

            //return View("CompanyProfile");
            return View(companyprofile);
        }


        [HttpPost]
        [Route("Company")]
        public string SaveCompanyProfile(POSAccount.Contract.Company company)
        {

            company.CreatedBy = Utility.DEFAULTUSER;
            company.ModifiedBy = Utility.DEFAULTUSER;
            company.IsActive = Utility.DEFAULTSTATUS;

            if (company.CompanyAddress.AddressId == 0 || company.CompanyAddress.AddressId == null)
            {
                company.CompanyAddress.AddressType = "Company";
                company.CompanyAddress.SeqNo = 1;
                company.CompanyAddress.AddressLinkID = company.CompanyCode;

            }

            var result = new POSAccount.BusinessFactory.CompanyBO().SaveCompany(company);


            return "Company details Saved.";
        }


        [Route("Branch")]
        public ActionResult BranchProfile()
        {
            var branchprofile = new POSAccount.BusinessFactory.BranchBO().GetBranch(new POSAccount.Contract.Branch { BranchCode = "VL" });

            //return View("CompanyProfile");
            return View(branchprofile);
        }


        [HttpPost]
        [Route("Branch")]
        public string SaveBranchProfile(POSAccount.Contract.Branch branch)
        {

            branch.CreatedBy = Utility.DEFAULTUSER;
            branch.ModifiedBy = Utility.DEFAULTUSER;
            branch.IsActive = Utility.DEFAULTSTATUS;

            if (branch.BranchAddress.AddressId == 0 || branch.BranchAddress.AddressId == null)
            {
                branch.BranchAddress.AddressType = "Branch";
                branch.BranchAddress.SeqNo = 1;
                branch.BranchAddress.IsActive = true;
                branch.BranchAddress.AddressLinkID = branch.BranchCode;

            }

            var result = new POSAccount.BusinessFactory.BranchBO().SaveBranch(branch);


            return "Company details Saved.";
        }

        */



        #region Company




        [Route("Company")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CompanyProfile(string companyCode)
        {
            Company company;
            if (companyCode != null)
                company = new POSAccount.BusinessFactory.CompanyBO().GetCompany(new Company { CompanyCode = companyCode });
            else
                company = new Company();

            company.CountryList = Utility.GetCountryList();
            //return View("CompanyProfile");
            return View(company);
        }

        [ChildActionOnly]
        [Route("GetCompanies")]
        public PartialViewResult GetCompanies()
        {
            var companyList = new POSAccount.BusinessFactory.CompanyBO().GetList();
            return PartialView(companyList);
        }




        [HttpPost]
        [Route("SaveCompanyProfile")]
        public ActionResult SaveCompanyProfile(POSAccount.Contract.Company company)
        {

            company.CreatedBy = Utility.DEFAULTUSER;
            company.ModifiedBy = Utility.DEFAULTUSER;
            company.IsActive = Utility.DEFAULTSTATUS;


            if (company.CompanyAddress.AddressId == 0 || company.CompanyAddress.AddressId == null)
            {
                company.CompanyAddress.AddressType = "Company";
                company.CompanyAddress.SeqNo = 1;
                company.CompanyAddress.AddressLinkID = company.CompanyCode;

            }

            var result = new POSAccount.BusinessFactory.CompanyBO().SaveCompany(company);


            //return RedirectToAction("CompanyProfile", "MasterData", new { companyCode = company.CompanyCode });

            return RedirectToAction("CompanyProfile", "MasterData");
        }

        #endregion

        #region Branch




        [Route("Branch")]
        [HttpGet]
        public ActionResult BranchProfile(string BranchCode)
        {
            var branchprofile = new POSAccount.BusinessFactory.BranchBO().GetBranch(new POSAccount.Contract.Branch { BranchCode = BranchCode });
            return View(branchprofile);
        }

        [HttpPost]
        [Route("SaveBranchProfile")]
        public ActionResult SaveBranchProfile(POSAccount.Contract.Branch branch)
        {

            branch.CreatedBy = Utility.DEFAULTUSER;
            branch.ModifiedBy = Utility.DEFAULTUSER;
            branch.IsActive = Utility.DEFAULTSTATUS;

            if (branch.BranchAddress.AddressId == 0 || branch.BranchAddress.AddressId == null)
            {
                branch.BranchAddress.AddressType = "Branch";
                branch.BranchAddress.SeqNo = 1;
                branch.BranchAddress.IsActive = true;
                branch.BranchAddress.AddressLinkID = branch.BranchCode;

            }
            var result = new POSAccount.BusinessFactory.BranchBO().SaveBranch(branch);
            //return RedirectToAction("Branch", "MasterData", new { BranchCode = branch.BranchCode });
            return RedirectToAction("CompanyProfile", "MasterData");
        }


        #endregion

        [Route("ChargeCodeMaster")]
        public ActionResult ChargeCodeMaster()
        {
            var chargeCodes = new POSAccount.BusinessFactory.ChargeCodeBO().GetList(Utility.SsnBranch).Where(x=> x.Status==true).ToList();

            return View("ChargeCode", chargeCodes);

            //EditChargeCode
        }

        [Route("EditChargeCode")]
        [HttpGet]
        public ActionResult EditChargeCode(string chargeCode)
        {
            var chargecodeItem = new POSAccount.Contract.ChargeCodeMaster();


            if (chargeCode != null && chargeCode.Length > 0)
                chargecodeItem = new POSAccount.BusinessFactory.ChargeCodeBO().GetChargeCode(new Contract.ChargeCodeMaster { ChargeCode = chargeCode, BranchID = Utility.SsnBranch });

            chargecodeItem.BillingUnitList = Utility.GetLookupItemList("UOM");
            chargecodeItem.CreditTermList = Utility.GetLookupItemList("CreditTerm");
            chargecodeItem.PaymentTermList = Utility.GetLookupItemList("PaymentTerm");
            chargecodeItem.AccountCodeList = Utility.GetAccountCodeItemList();
            chargecodeItem.VATAccountCodeList = Utility.GetAccountCodeItemList();
            chargecodeItem.WHAccountCodeList = Utility.GetAccountCodeItemList();


            //return PartialView("AddChargeCode", chargecodeItem);
            return View("NewChargeCode", chargecodeItem);
        }



        [Route("EditChargeCode")]
        [HttpPost]
        public ActionResult UpdateChargeCode(POSAccount.Contract.ChargeCodeMaster item)
        {

            if (ModelState.IsValid)
            {

                if (item == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                item.CreatedBy = Utility.DEFAULTUSER;
                item.ModifiedBy = Utility.DEFAULTUSER;
                item.BranchID = Utility.SsnBranch;

                var result = new POSAccount.BusinessFactory.ChargeCodeBO().SaveChargeCode(item);
            }


            return RedirectToAction("ChargeCodeMaster", "MasterData");

        }

        [Route("SaveChargeCode")]
        [HttpPost]
        public JsonResult SaveChargeCode(POSAccount.Contract.ChargeCodeMaster item)
        {

            if (ModelState.IsValid)
            {

                if (item == null)
                    return Json(new { Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);

                item.CreatedBy = Utility.DEFAULTUSER;
                item.ModifiedBy = Utility.DEFAULTUSER;
                item.Status = true;
                item.BranchID = Utility.SsnBranch;

                var result = new POSAccount.BusinessFactory.ChargeCodeBO().SaveChargeCode(item);
            }

            //return RedirectToAction("ChargeCodeMaster", "MasterData");
            return Json(new { success = true, Message = string.Format("Charge Code {0} saved successfully.", item.ChargeCode), ChargeCodeMaster = item });

        }


        [Route("DeleteChargeCode")]
        [HttpPost]
        public JsonResult DeleteChargeCode(POSAccount.Contract.ChargeCodeMaster item)
        {
            try
            {
                if (item != null)
                {
                    item.CreatedBy = Utility.DEFAULTUSER;
                    item.ModifiedBy = Utility.DEFAULTUSER;
                    item.Status = false;
                    item.BranchID = Utility.SsnBranch;
                    //var chargecodeItem = new POSAccount.BusinessFactory.ChargeCodeBO().GetChargeCode(new Contract.ChargeCodeMaster { ChargeCode = chargeCode, BranchID = Utility.SsnBranch });
                    var result = new POSAccount.BusinessFactory.ChargeCodeBO().DeleteChargeCode(item);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return Json(new { success = true, Message = string.Format("Charge Code {0} deleted successfully.", item.ChargeCode), ChargeCodeMaster = item });
        }


        [Route("UOMMaster")]
        public ActionResult UOMMaster()
        {
            return View("UOM");
        }

        protected List<ChartOfAccountsVm> PopulateChilds(string code, List<POSAccount.Contract.ChartOfAccount> chartOfAccountList)
        {
            var obj = chartOfAccountList.Where(x => x.AccountGroup == code)
                                        .Select(x => new ChartOfAccountsVm()
                                        {
                                            AccountCode = x.AccountCode,
                                            Description = x.Description,
                                            Description2 = x.Description2,
                                            AccountGroup = x.AccountGroup,
                                            DebitCredit = x.DebitCredit,
                                            CurrencyCode = x.CurrencyCode,
                                            Status = x.Status,
                                            Sequence = x.Sequence,
                                            BranchID = x.BranchID,
                                            CreatedBy = x.CreatedBy,
                                            CreatedOn = x.CreatedOn,
                                            ModifiedBy = x.ModifiedBy,
                                            ModifiedOn = x.ModifiedOn,
                                            AccountGroupDescription = x.AccountGroupDescription,
                                            COAList = PopulateChilds(x.AccountCode, chartOfAccountList)
                                        }).ToList<ChartOfAccountsVm>();

            return obj;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Route("ChartOfAccountMaster")]
        public ActionResult ChartOfAccountMaster(string branchID, string _accountCode = "")
        {
            var accountCode = Request.Form["hdnAccountCode"];
            if (string.IsNullOrWhiteSpace(accountCode))
            {
                if (!string.IsNullOrWhiteSpace(_accountCode))
                {
                    accountCode = _accountCode;
                }
            }
            ViewData["hdnAccountCode"] = accountCode;

            short branchId = -1;
            if (branchID != null)
            {
                branchId = Convert.ToInt16(branchID);
            }
            else
            {
                branchId = Convert.ToInt16(Session["BranchId"]);
            }
            //var ChartOfAccounts = new POSAccount.BusinessFactory.ChartOfAccountBO().GetList();
            var accountGroupList = new POSAccount.BusinessFactory.AccountGroupBO().GetList();
            var chartOfAccountsList = new POSAccount.BusinessFactory.ChartOfAccountBO().GetList(branchId);

            List<AccountGroupVm> accountGroupVm = new List<AccountGroupVm>();
            for (var i = 0; i < accountGroupList.Count; i++)
            {
                var code = accountGroupList[i].Code;
                AccountGroupVm accountGroup = new AccountGroupVm();
                accountGroup.Code = accountGroupList[i].Code;
                accountGroup.AccountType = accountGroupList[i].AccountType;
                accountGroup.Description = accountGroupList[i].Description;
                accountGroup.Description1 = accountGroupList[i].Description1;
                accountGroup.Description2 = accountGroupList[i].Description2;
                accountGroup.SequenceNo = accountGroupList[i].SequenceNo;
                
                accountGroup.COAList = chartOfAccountsList.Where(x => x.AccountGroup == accountGroupList[i].Code)
                                                          .Select(x => new ChartOfAccountsVm()
                                                          {
                                                              AccountCode = x.AccountCode,
                                                              Description = x.Description,
                                                              Description2 = x.Description2,
                                                              AccountGroup = x.AccountGroup,
                                                              DebitCredit = x.DebitCredit,
                                                              CurrencyCode = x.CurrencyCode,
                                                              Status = x.Status,
                                                              Sequence = x.Sequence,
                                                              BranchID = x.BranchID,
                                                              CreatedBy = x.CreatedBy,
                                                              CreatedOn = x.CreatedOn,
                                                              ModifiedBy = x.ModifiedBy,
                                                              ModifiedOn = x.ModifiedOn,
                                                              AccountGroupDescription = x.AccountGroupDescription,
                                                              COAList = PopulateChilds(x.AccountCode, chartOfAccountsList)
                                                          }).ToList<ChartOfAccountsVm>();

                accountGroupVm.Add(accountGroup);
            }
            /*
            var list = ChartOfAccounts.GroupBy(x => x.AccountGroup)
                                        .Select(x => new MyItem() {
                                            Description = x.FirstOrDefault().Description,
                                            Description2 = x.FirstOrDefault().Description2,
                                            AccountGroupDescription = x.FirstOrDefault().AccountGroupDescription,
                                            Sequence = x.FirstOrDefault().Sequence,
                                            AccountCode = x.FirstOrDefault().AccountCode,
                                            ChildItems = (ChartOfAccounts.Where(y => y.AccountGroup == x.FirstOrDefault().AccountGroup)
                                                            .Select(y => new MyItem() {
                                                                Description = y.Description,
                                                                Description2 = y.Description2,
                                                                AccountGroupDescription = y.AccountGroupDescription,
                                                                Sequence = y.Sequence,
                                                                AccountCode = y.AccountCode
                                                            })).ToList()
                                        }).ToList<MyItem>();*/

            if (Request.IsAjaxRequest())
            {
                return Json(accountGroupVm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("ChartOfAccounts", accountGroupVm);
            }            
        }

        [Route("EditChartOfAccount")]
        [ChildActionOnly]
        public ActionResult EditChartOfAccount(string accountCode)
        {
            return PartialView("ChartOfAccount", ChartOfAccountData(accountCode));
        }

        public ChartOfAccount ChartOfAccountData(string accountCode)
        {
            var coaItem = new POSAccount.Contract.ChartOfAccount();

            if (accountCode != null && accountCode.Length > 0)
                coaItem = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(new Contract.ChartOfAccount { AccountCode = accountCode, BranchID = Utility.SsnBranch });

            coaItem.CurrencyCodeList = Utility.GetCurrencyItemList();
            coaItem.AccountGroupList = Utility.GetAccountGroupItemList(Convert.ToInt16((Session["BranchId"]))).Where(x => x.Value != accountCode).ToList();
            coaItem.DebitCreditList = Utility.GetLookupItemList("DebitCredit");

            return coaItem;
        }

        [HttpGet]
        [Route("chartofaccount/{accountCode}")]
        public JsonResult ChartOfAccountByCode(string accountCode)
        {
            return Json(ChartOfAccountData(accountCode), JsonRequestBehavior.AllowGet);
        }

        [Route("EditChartOfAccount")]
        [HttpPost]
        public ActionResult UpdateChartOfAccount(POSAccount.Contract.ChartOfAccount item)
        {

            if (ModelState.IsValid)
            {
                if (item.BranchID == 0)
                {
                    item.BranchID = Utility.SsnBranch;
                }
                if (item == null)
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

                item.CreatedBy = Utility.DEFAULTUSER;
                item.ModifiedBy = Utility.DEFAULTUSER;

                var result = new POSAccount.BusinessFactory.ChartOfAccountBO().SaveChartOfAccount(item);                
            }            
            return RedirectToAction("ChartOfAccountMaster", "MasterData");
        }

        [Route("DeleteChartOfAccount")]
        [HttpPost]
        public ActionResult DeleteChartOfAccount(POSAccount.Contract.ChartOfAccount item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = new POSAccount.BusinessFactory.ChartOfAccountBO().DeleteChartOfAccount(item);
                }
                catch (Exception ex)
                {
                    TempData["TempExMsg"] = ex.Message;                    
                }
                
            }
            return RedirectToAction("ChartOfAccountMaster", "MasterData", new { branchID = item.BranchID, _accountCode = "" });
        }

        [Route("DashBoard")]
        public ActionResult DashBoard()
        {

            return View("DashBoard");
        }

        #region Asset Master




        [Route("AssetMaster")]
        public ActionResult AssetMaster()
        {
            var assets = new POSAccount.BusinessFactory.AssetHeaderBO().GetList(Utility.SsnBranch).Where(x=>x.Status==true).ToList();

            

            return View("AssetMaster", assets);

            //EditChargeCode
        }

        

        [Route("EditAsset")]
        [HttpGet]
        public ActionResult EditAsset(string assetCode)
        {
            var assetItem = new POSAccount.Contract.AssetHeader();


            if (assetCode != null && assetCode.Length > 0)
                assetItem = new POSAccount.BusinessFactory.AssetHeaderBO().GetAssetHeader(new AssetHeader { AssetCode = assetCode, BranchID = Utility.SsnBranch });
            else
                assetItem.BuyingDate = DateTime.Today.Date;

            assetItem.DepreciationTypeList = Utility.GetLookupItemList("Depreciation");

            return View("AddAsset", assetItem);
        }


        [HttpPost]
        [Route("SaveAsset")]
        public ActionResult SaveAsset(POSAccount.Contract.AssetHeader assetHeader)
        {

            assetHeader.CreatedBy = Utility.DEFAULTUSER;
            assetHeader.ModifiedBy = Utility.DEFAULTUSER;
            assetHeader.Status = Utility.DEFAULTSTATUS;
            assetHeader.BranchID = Utility.SsnBranch;


            var result = new POSAccount.BusinessFactory.AssetHeaderBO().SaveAssetHeader(assetHeader);

            //return RedirectToAction("AssetMaster", "MasterData");
            return Json(new { success = true, Message = string.Format("Asset Code {0} saved successfully.", assetHeader.AssetCode) });
        }


        [HttpPost]
        [Route("DeleteAsset")]
        public ActionResult DeleteAsset(POSAccount.Contract.AssetHeader assetHeader)
        {
            if (assetHeader != null)
            {
                try
                {
                    assetHeader.CreatedBy = Utility.DEFAULTUSER;
                    assetHeader.ModifiedBy = Utility.DEFAULTUSER;
                    assetHeader.Status = false;
                    assetHeader.BranchID = Utility.SsnBranch;
                    var item = new POSAccount.BusinessFactory.AssetHeaderBO().DeleteAssetHeader(assetHeader);
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }

            var assets = new POSAccount.BusinessFactory.AssetHeaderBO().GetList(Utility.SsnBranch);

           // return View("AssetMaster", assets);
            return Json(new { success = true, Message = string.Format("Asset Code {0} deleted successfully.", assetHeader.AssetCode) });
        }

        #endregion


    }

    public class AccountGroupVm
    {
        public string Code { get; set; }
        public string AccountType { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public Int16 SequenceNo { get; set; }
        public List<ChartOfAccountsVm> COAList { get; set; }
    }

    public class ChartOfAccountsVm
    {
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string AccountGroup { get; set; }
        public string DebitCredit { get; set; }
        public string CurrencyCode { get; set; }
        public bool Status { get; set; }
        public Int16 Sequence { get; set; }
        public Int16 BranchID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string AccountGroupDescription { get; set; }
        public List<ChartOfAccountsVm> COAList { get; set; }
    }
    /*
    public class MyItem
    {
        public string AccountCode { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string AccountGroupDescription { get; set; }
        public Int16 Sequence { get; set; }
        public List<MyItem> ChildItems { get; set; }
    }
    */

}
