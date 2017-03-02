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
    public class CBReceiptBO
    {
        private CBReceiptDAL cbreceiptDAL;
        public CBReceiptBO()
        {

            cbreceiptDAL = new CBReceiptDAL();
        }

        public List<CBReceipt> GetList()
        {
            return cbreceiptDAL.GetList();
        }


        public bool SaveCBReceipt(CBReceipt newItem)
        {

            return cbreceiptDAL.Save(newItem);

        }

        public bool DeleteCBReceipt(CBReceipt item)
        {
            return cbreceiptDAL.Delete(item);
        }
        
        public bool DeleteCBReceipt(string documentNo, string cancelledBy)
        {
            return cbreceiptDAL.Delete(documentNo,cancelledBy);
        }

        


        public CBReceipt GetCBReceipt(CBReceipt item)
        {
            return (CBReceipt)cbreceiptDAL.GetItem<CBReceipt>(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return cbreceiptDAL.PerformSearch(whereclause);
        }
    }
}
