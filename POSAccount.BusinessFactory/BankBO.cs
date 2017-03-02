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
    public class BankBO
    {
        private BankDAL bankDAL;
        public BankBO()
        {

            bankDAL = new BankDAL();
        }

        public List<Bank> GetList()
        {
            return bankDAL.GetList();
        }


        public bool SaveBank(Bank newItem)
        {

            return bankDAL.Save(newItem);

        }

        public bool DeleteBank(Bank item)
        {
            return bankDAL.Delete(item);
        }

        public Bank GetBank(Bank item)
        {
            return (Bank)bankDAL.GetItem<Bank>(item);
        }


        public Address GetBankAddress(Bank item)
        {
            return bankDAL.GetBankAddress(item);
        }

        public List<BankRecon> GetBankReconciliationList(short branchId, string bankCode, DateTime dateFrom, DateTime dateTo)
        {
            return new GLTransactionDAL().GetBankReconciliationList(branchId, bankCode, dateFrom, dateTo);
        }

    }
}
