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
    public class BranchBO
    {
        private BranchDAL branchDAL;
        public BranchBO()
        {

            branchDAL = new BranchDAL();
        }

        public List<Branch> GetList()
        {
            return branchDAL.GetList();
        }


        public bool SaveBranch(Branch newItem)
        {

            return branchDAL.Save(newItem);

        }

        public bool DeleteBranch(Branch item)
        {
            return branchDAL.Delete(item);
        }

        public Branch GetBranch(Branch item)
        {
            return (Branch)branchDAL.GetItem<Branch>(item);
        }

    }
}
