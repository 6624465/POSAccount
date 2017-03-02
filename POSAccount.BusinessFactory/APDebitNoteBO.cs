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
    public class APDebitNoteBO
    {
        private APDebitNoteDAL apdebitnoteDAL;
        public APDebitNoteBO()
        {

            apdebitnoteDAL = new APDebitNoteDAL();
        }

        public List<APDebitNote> GetList(short branchID)
        {
            return apdebitnoteDAL.GetList(branchID);
        }


        public bool SaveAPDebitNote(APDebitNote newItem)
        {

            return apdebitnoteDAL.Save(newItem);

        }

        public bool DeleteAPDebitNote(APDebitNote item)
        {
            return apdebitnoteDAL.Delete(item);
        }

        public APDebitNote GetAPDebitNote(APDebitNote item)
        {
            return (APDebitNote)apdebitnoteDAL.GetItem<APDebitNote>(item);
        }

    }
}
