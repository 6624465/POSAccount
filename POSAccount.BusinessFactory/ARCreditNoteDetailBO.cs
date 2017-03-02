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
    public class ARCreditNoteDetailBO
    {
        private ARCreditNoteDetailDAL arcreditnotedetailDAL;
        public ARCreditNoteDetailBO()
        {

            arcreditnotedetailDAL = new ARCreditNoteDetailDAL();
        }

        public List<ARCreditNoteDetail> GetList()
        {
            return arcreditnotedetailDAL.GetList();
        }


        public bool SaveARCreditNoteDetail(ARCreditNoteDetail newItem)
        {

            return arcreditnotedetailDAL.Save(newItem);

        }

        public bool DeleteARCreditNoteDetail(ARCreditNoteDetail item)
        {
            return arcreditnotedetailDAL.Delete(item);
        }

        public ARCreditNoteDetail GetARCreditNoteDetail(ARCreditNoteDetail item)
        {
            return (ARCreditNoteDetail)arcreditnotedetailDAL.GetItem<ARCreditNoteDetail>(item);
        }

    }
}
