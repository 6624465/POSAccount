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
    public class ARCreditNoteBO
    {
        private ARCreditNoteDAL arcreditnoteDAL;
        public ARCreditNoteBO()
        {

            arcreditnoteDAL = new ARCreditNoteDAL();
        }

        public List<ARCreditNote> GetList()
        {
            return arcreditnoteDAL.GetList();
        }


        public bool SaveARCreditNote(ARCreditNote newItem)
        {

            return arcreditnoteDAL.Save(newItem);

        }

        public bool DeleteARCreditNote(ARCreditNote item)
        {
            return arcreditnoteDAL.Delete(item);
        }

        public ARCreditNote GetARCreditNote(ARCreditNote item)
        {
            return (ARCreditNote)arcreditnoteDAL.GetItem<ARCreditNote>(item);
        }

    }
}
