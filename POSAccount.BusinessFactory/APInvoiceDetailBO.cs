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
    public class APInvoiceDetailBO
    {
        private APInvoiceDetailDAL apinvoicedetailDAL;
        public APInvoiceDetailBO()
        {

            apinvoicedetailDAL = new APInvoiceDetailDAL();
        }

        public List<APInvoiceDetail> GetList()
        {
            return apinvoicedetailDAL.GetList();
        }

        public List<APInvoiceDetail> GetListByDocumentNo(string documentNo)
        {
            return apinvoicedetailDAL.GetListByDocumentNo(documentNo);
        }

        public bool SaveAPInvoiceDetail(APInvoiceDetail newItem)
        {

            return apinvoicedetailDAL.Save(newItem);

        }

        public bool DeleteAPInvoiceDetail(APInvoiceDetail item)
        {
            return apinvoicedetailDAL.Delete(item);
        }

        public APInvoiceDetail GetAPInvoiceDetail(APInvoiceDetail item)
        {
            return (APInvoiceDetail)apinvoicedetailDAL.GetItem<APInvoiceDetail>(item);
        }

    }
}
