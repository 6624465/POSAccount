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
    public class APDebitNoteDetailBO
    {
        private APDebitNoteDetailDAL apdebitnotedetailDAL;
        public APDebitNoteDetailBO()
        {

            apdebitnotedetailDAL = new APDebitNoteDetailDAL();
        }

        public List<APDebitNoteDetail> GetList()
        {
            return apdebitnotedetailDAL.GetList();
        }


        public bool SaveAPDebitNoteDetail(APDebitNoteDetail newItem)
        {

            return apdebitnotedetailDAL.Save(newItem);

        }

        public bool DeleteAPDebitNoteDetail(APDebitNoteDetail item)
        {
            return apdebitnotedetailDAL.Delete(item);
        }

        public APDebitNoteDetail GetAPDebitNoteDetail(APDebitNoteDetail item)
        {
            return (APDebitNoteDetail)apdebitnotedetailDAL.GetItem<APDebitNoteDetail>(item);
        }

    }
}
