using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;

namespace POSAccount.BusinessFactory
{
    public class ChartOfAccountBO
    {
        private ChartOfAccountDAL chartofaccountDAL;
        public ChartOfAccountBO()
        {

            chartofaccountDAL = new ChartOfAccountDAL();
        }

        public List<ChartOfAccount> GetList(short branchID)
        {
            return chartofaccountDAL.GetList(branchID);
        }

        public List<ChartOfAccount> GetCashBankAccountList()
        {
            return chartofaccountDAL.GetCashBankAccountList();
        }

        public List<ChartOfAccount> GetCreditorAccountList()
        {
            return chartofaccountDAL.GetCreditorAccountList();
        }

        public List<ChartOfAccount> GetDebtorAccountList()
        {
            return chartofaccountDAL.GetDebtorAccountList();
        }



        public bool SaveChartOfAccount(ChartOfAccount newItem)
        {

            return chartofaccountDAL.Save(newItem);

        }

        public bool DeleteChartOfAccount(ChartOfAccount item)
        {
            try
            {
                return chartofaccountDAL.Delete(item);
            }
            catch (System.Exception ex)
            {                
                throw ex;
            }
            
        }

        public ChartOfAccount GetChartOfAccount(ChartOfAccount item)
        {
            return (ChartOfAccount)chartofaccountDAL.GetItem<ChartOfAccount>(item);
        }

    }
}
