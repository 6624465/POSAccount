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
    public class APCreditNoteDetailBO
    {
        private APCreditNoteDetailDAL apcreditnotedetailDAL;
        public APCreditNoteDetailBO()
        {

            apcreditnotedetailDAL = new APCreditNoteDetailDAL();
        }

        public List<APCreditNoteDetail> GetList()
        {
            return apcreditnotedetailDAL.GetList();
        }


        public bool SaveAPCreditNoteDetail(APCreditNoteDetail newItem)
        {

            return apcreditnotedetailDAL.Save(newItem);

        }

        public bool DeleteAPCreditNoteDetail(APCreditNoteDetail item)
        {
            return apcreditnotedetailDAL.Delete(item);
        }

        public APCreditNoteDetail GetAPCreditNoteDetail(APCreditNoteDetail item)
        {
            return (APCreditNoteDetail)apcreditnotedetailDAL.GetItem<APCreditNoteDetail>(item);
        }

    }
}
