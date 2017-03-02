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
    public class ARDebitNoteBO
    {
        private ARDebitNoteDAL ardebitnoteDAL;
        public ARDebitNoteBO()
        {

            ardebitnoteDAL = new ARDebitNoteDAL();
        }

        public List<ARDebitNote> GetList()
        {
            return ardebitnoteDAL.GetList();
        }


        public bool SaveARDebitNote(ARDebitNote newItem)
        {

            return ardebitnoteDAL.Save(newItem);

        }

        public bool DeleteARDebitNote(ARDebitNote item)
        {
            return ardebitnoteDAL.Delete(item);
        }

        public ARDebitNote GetARDebitNote(ARDebitNote item)
        {
            return (ARDebitNote)ardebitnoteDAL.GetItem<ARDebitNote>(item);
        }

    }
}
