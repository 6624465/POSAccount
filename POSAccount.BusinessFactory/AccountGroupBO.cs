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
    public class AccountGroupBO
    {
        private AccountGroupDAL accountgroupDAL;
        public AccountGroupBO()
        {

            accountgroupDAL = new AccountGroupDAL();
        }

        public List<AccountGroup> GetList(short branchID)
        {
            return accountgroupDAL.GetList(branchID);
        }

        public List<AccountGroup> GetList()
        {
            return accountgroupDAL.GetList();
        }

        public bool SaveAccountGroup(AccountGroup newItem)
        {

            return accountgroupDAL.Save(newItem);

        }

        public bool DeleteAccountGroup(AccountGroup item)
        {
            return accountgroupDAL.Delete(item);
        }

        public AccountGroup GetAccountGroup(AccountGroup item, short branchID)
        {
            return (AccountGroup)accountgroupDAL.GetItem<AccountGroup>(item,branchID);
        }

    }
}
