using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Data;

namespace POSAccount.BusinessFactory
{
    public class CBBankTransferBO
    {
        private CBBankTransferDAL   cbbanktransferDAL;
        public CBBankTransferBO() {

            cbbanktransferDAL = new CBBankTransferDAL();
        }

        public List<CBBankTransfer> GetList(short branchID)
        {
            return cbbanktransferDAL.GetList(branchID);
        }


        public bool SaveCBBankTransfer(CBBankTransfer newItem)
        {

            return cbbanktransferDAL.Save(newItem);

        }

        public bool DeleteCBBankTransfer(CBBankTransfer item)
        {
            return cbbanktransferDAL.Delete(item);
        }

        public CBBankTransfer GetCBBankTransfer(CBBankTransfer item)
        {
            return (CBBankTransfer)cbbanktransferDAL.GetItem<CBBankTransfer>(item);
        }

    }
}
