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
    public class ChargeCodeBO
    {
        private ChargeCodeDAL chargecodeDAL;
        public ChargeCodeBO()
        {

            chargecodeDAL = new ChargeCodeDAL();
        }

        public List<ChargeCodeMaster> GetList(short branchID)
        {
            return chargecodeDAL.GetList(branchID);
        }


        public bool SaveChargeCode(ChargeCodeMaster newItem)
        {

            return chargecodeDAL.Save(newItem);

        }

        public bool DeleteChargeCode(ChargeCodeMaster item)
        {
            return chargecodeDAL.Delete(item);
        }

        public ChargeCodeMaster GetChargeCode(ChargeCodeMaster item)
        {
            return (ChargeCodeMaster)chargecodeDAL.GetItem<ChargeCodeMaster>(item);
        }

    }
}
