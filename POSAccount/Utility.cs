using POSAccount.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace POSAccount
{

    public enum DefaultAccountCodes
    {
        BANKCHARGESACCOUNTCODE = 1,
        ARVATACCOUNTCODE = 2,
        ARWHACCOUNTCODE = 3,
        APVATACCOUNTCODE = 4,
        APWHACCOUNTCODE = 5,
        ARADVANCEVATACCOUNTCODE=6

    };

    public static class Utility
    {
        public static string DEFAULTUSER = "SYSTEM";
        public static bool DEFAULTSTATUS = true;
        public static string DEFAULTUSERNAME = "SYSTEM";
        public static string NEWRECORD = "NEW";
        public static string DEFAULTCURRENCYCODE = "THB";
        public static string USERROLE = "";

        public static string BANKCHARGESACCOUNTCODE = "5310-04";

        /* AR - CB RECEIPT */
        public static string ARVATACCOUNTCODE = "2300-01";
        public static string ARWHACCOUNTCODE = "1150-01";

        /* AP - CB PAYMENT */
        public static string APVATACCOUNTCODE = "1150-02";
        public static string APWHACCOUNTCODE = "2120-01";
        public static string ARADVANCEVATACCOUNTCODE = "1150-03";
        


        public static string REPORTSUBFOLDER = WebConfigurationManager.AppSettings["ReportSubFolder"].ToString();

        /*
         * 
         * this is an Extension Method.
         * 
         * Usage :  DateTime.Now.Date.ThaiTime();
         * 
         * 
         */

        public static DateTime ThaiTime(this DateTime value)
        {
            string nzTimeZoneKey = "SE Asia Standard Time";
            DateTime utc = DateTime.UtcNow;

            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
            return TimeZoneInfo.ConvertTimeFromUtc(value, nzTimeZone);
        }

        public static string GetDefaultAccountCodes(DefaultAccountCodes code) {

            var lstSystemWideConfiguration = new POSAccount.BusinessFactory.SystemWideConfigurationBO().GetList();

            var value = "";
            var config = "";

            switch (code)
            {
                case DefaultAccountCodes.BANKCHARGESACCOUNTCODE:
                    config = "Bank Fee Account Code";
                    break;
                case DefaultAccountCodes.ARVATACCOUNTCODE:
                    config = "AR VAT Account Code";
                    break;
                case DefaultAccountCodes.ARWHACCOUNTCODE:
                    config = "AR W/H Account Code";
                    break;
                case DefaultAccountCodes.APVATACCOUNTCODE:
                    config = "AP VAT Account Code";
                    break;
                case DefaultAccountCodes.APWHACCOUNTCODE:
                    config = "AP W/H Account Code";
                    break;

                case DefaultAccountCodes.ARADVANCEVATACCOUNTCODE:
                    config = "AR Adv. VAT Account Code";
                    break;
                    
                default:
                    break;
            }
            value = lstSystemWideConfiguration.Where(x => x.DisplayName == config).FirstOrDefault().ConfigurationValue;

            return value;
        }

        public static IEnumerable<SelectListItem> GetCurrencyItemList()
        {
            var lstcurrency = new POSAccount.BusinessFactory.CurrencyBO().GetList();

            if (lstcurrency == null)
                return null;

            var selectList = lstcurrency.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.CurrencyCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");


        }

        public static short SsnBranch
        {
            get 
            {
                return Convert.ToInt16(HttpContext.Current.Session["BranchId"]);
            }
        }


        public static IEnumerable<SelectListItem> GetYearList()
        {
            List<SelectListItem> lstYear = new List<SelectListItem>();
            for (int i = 2010; i <= 2016; i++)
			{
			    lstYear.Add(new SelectListItem { Text = i.ToString(), Value=i.ToString() }); 
			}
            return new SelectList(lstYear, "Value", "Text");
        }


        public static IEnumerable<SelectListItem> GetPeriodList()
        {
            List<SelectListItem> lstPeriod = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                lstPeriod.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return new SelectList(lstPeriod, "Value", "Text");


        }

        public static IEnumerable<SelectListItem> GetRoleList()
        {


            var lstRole = new POSAccount.BusinessFactory.RolesBO().GetList();
            var selectList = lstRole.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.RoleCode,
                                    Text = c.RoleDescription    
                                });


            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetAccountCodeItemList()
        {
            var lstaccountcode = new POSAccount.BusinessFactory.ChartOfAccountBO().GetList(SsnBranch);

            if (lstaccountcode == null)
                return null;

            var selectList = lstaccountcode.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.AccountCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");


        }

        public static IEnumerable<SelectListItem> GetChargeCodeItemList()
        {
            var lstchargecode = new POSAccount.BusinessFactory.ChargeCodeBO().GetList(Utility.SsnBranch);

            if (lstchargecode == null)
                return null;

            var selectList = lstchargecode.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.ChargeCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");


        }


        public static IEnumerable<SelectListItem> GetAccountGroupItemList(short branchID)
        {
            var lstAccountGroup = new POSAccount.BusinessFactory.AccountGroupBO().GetList();

            var lstCoa = new POSAccount.BusinessFactory.ChartOfAccountBO().GetList(branchID).Select(x => new SelectListItem{ Value = x.AccountCode, Text = x.Description }).ToList();


            

            if (lstAccountGroup == null)
                return null;

            var selectList = lstAccountGroup.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.Code,
                                    Text = c.Description
                                }).ToList();

            selectList.AddRange(lstCoa);

            return new SelectList(selectList, "Value", "Text");


        }


        public static IEnumerable<SelectListItem> GetLookupItemList(string lookupCategory)
        {
            var lstLookUp = new POSAccount.BusinessFactory.LookUpBO().GetList().Where(lk => lk.Category == lookupCategory).ToList();

            if (lstLookUp == null)
                return null;

            var selectList = lstLookUp.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.LookupCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");


        }

        public static IEnumerable<SelectListItem> GetCountryList()
        {
            var countryList = new POSAccount.BusinessFactory.CountryBO().GetList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.CountryCode,
                                    Text = c.CountryName
                                });

            return new SelectList(selectList, "Value", "Text");

        }


        public static IEnumerable<SelectListItem> GetPaymentTypeList(string cbCategory)
        {
            var lstLookUp = new POSAccount.BusinessFactory.LookUpBO().GetList().Where(lk => lk.Category == cbCategory).ToList();

            if (lstLookUp == null)
                return null;

            var selectList = lstLookUp.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.LookupCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetReceiptTypeList(string cbCategory)
        {
            var lstLookUp = new POSAccount.BusinessFactory.LookUpBO().GetList().Where(lk => lk.Category == cbCategory).ToList();

            if (lstLookUp == null)
                return null;

            var selectList = lstLookUp.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.LookupCode,
                                    Text = c.Description
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetBankCodeList()
        {
            var countryList = new POSAccount.BusinessFactory.BankBO().GetList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.BankCode,
                                    Text = string.Format("{0} - {1}", c.BankCode, c.Name)
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetCreditorList()
        {
            var countryList = new POSAccount.BusinessFactory.CreditorBO().GetList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.CreditorCode,
                                    Text = string.Format("{0} ({1})", c.CreditorName, c.CreditorCode)
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetDebtorList()
        {
            var countryList = new POSAccount.BusinessFactory.DebtorBO().GetList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.DebtorCode,
                                    Text = string.Format("{0} ({1})", c.DebtorName, c.DebtorCode)
                                });

            return new SelectList(selectList, "Value", "Text");

        }


        public static IEnumerable<SelectListItem> GetDebtorAccountList()
        {
            var countryList = new POSAccount.BusinessFactory.ChartOfAccountBO().GetDebtorAccountList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.AccountCode,
                                    Text = string.Format("{0} ({1})", c.AccountCode, c.Description)
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static IEnumerable<SelectListItem> GetDiscountList()
        {


            List<string> lstDiscount = new List<string>();

            lstDiscount.Add("NONE");
            lstDiscount.Add("AMOUNT");
            lstDiscount.Add("PERCENTAGE");


            var selectList = lstDiscount.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.ToString(),
                                    Text = c.ToString()
                                });


            return new SelectList(selectList, "Value", "Text");

        }


        public static IEnumerable<SelectListItem> GetCreditorAccountList()
        {
            var countryList = new POSAccount.BusinessFactory.ChartOfAccountBO().GetCreditorAccountList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.AccountCode,
                                    Text = string.Format("{0} ({1})", c.AccountCode, c.Description)
                                });

            return new SelectList(selectList, "Value", "Text");

        }



        public static IEnumerable<SelectListItem> GetCashBankAccountList()
        {
            var countryList = new POSAccount.BusinessFactory.ChartOfAccountBO().GetCashBankAccountList();

            var selectList = countryList.Select(c =>
                                new SelectListItem
                                {
                                    Value = c.AccountCode,
                                    Text = string.Format("{0} ({1})", c.AccountCode, c.Description)
                                });

            return new SelectList(selectList, "Value", "Text");

        }

        public static ChartOfAccount GetAccountType(string accountCode)
        {
            ChartOfAccount chartOfAccount = new ChartOfAccount();
            chartOfAccount.AccountCode = accountCode;
            chartOfAccount.BranchID = Utility.SsnBranch;
            var accountType = new POSAccount.BusinessFactory.ChartOfAccountBO().GetChartOfAccount(chartOfAccount);
            return accountType;

        }


       


        public static void Update<T>(this IEnumerable<T> source, params Action<T>[] updateValues)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (updateValues == null)
            {
                throw new ArgumentNullException("updateValues");
            }

            foreach (T item in source)
            {
                foreach (Action<T> update in updateValues)
                {
                    update(item);
                }
            }
        }

    }

    public static class ModelStateHelper
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Count() > 0);
            }
            return null;
        }
    }

      

    
}