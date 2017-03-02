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
    public class ARDebitNoteDetailBO
    {
        private ARDebitNoteDetailDAL ardebitnotedetailDAL;
        public ARDebitNoteDetailBO()
        {

            ardebitnotedetailDAL = new ARDebitNoteDetailDAL();
        }

        public List<ARDebitNoteDetail> GetList()
        {
            return ardebitnotedetailDAL.GetList();
        }


        public bool SaveARDebitNoteDetail(ARDebitNoteDetail newItem)
        {

            return ardebitnotedetailDAL.Save(newItem);

        }

        public bool DeleteARDebitNoteDetail(ARDebitNoteDetail item)
        {
            return ardebitnotedetailDAL.Delete(item);
        }

        public ARDebitNoteDetail GetARDebitNoteDetail(ARDebitNoteDetail item)
        {
            return (ARDebitNoteDetail)ardebitnotedetailDAL.GetItem<ARDebitNoteDetail>(item);
        }

    }
}
