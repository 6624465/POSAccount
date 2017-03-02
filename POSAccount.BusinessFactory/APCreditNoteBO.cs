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
    public class APCreditNoteBO
    {
        private APCreditNoteDAL apcreditnoteDAL;
        public APCreditNoteBO()
        {

            apcreditnoteDAL = new APCreditNoteDAL();
        }

        public List<APCreditNote> GetList(short branchID)
        {
            return apcreditnoteDAL.GetList(branchID);
        }


        public bool SaveAPCreditNote(APCreditNote newItem)
        {

            return apcreditnoteDAL.Save(newItem);

        }

        public bool DeleteAPCreditNote(APCreditNote item)
        {
            return apcreditnoteDAL.Delete(item);
        }

        public APCreditNote GetAPCreditNote(APCreditNote item)
        {
            return (APCreditNote)apcreditnoteDAL.GetItem<APCreditNote>(item);
        }

    }
}
