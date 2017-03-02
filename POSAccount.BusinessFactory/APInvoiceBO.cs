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
    public class APInvoiceBO
    {
        private APInvoiceDAL apinvoceDAL;
        public APInvoiceBO()
        {

            apinvoceDAL = new APInvoiceDAL();
        }

        public List<APInvoice> GetList()
        {
            return apinvoceDAL.GetList();
        }


        public List<APInvoice> GetListByDocumentNo(string documentNo) {
            return apinvoceDAL.GetListByDocumentNo(documentNo);
        }
        

        public bool SaveAPInvoice(APInvoice newItem)
        {

            return apinvoceDAL.Save(newItem);

        }

        public bool DeleteAPInvoice(APInvoice item)
        {
            return apinvoceDAL.Delete(item);
        }

        public APInvoice GetAPInvoice(APInvoice item)
        {
            return (APInvoice)apinvoceDAL.GetItem<APInvoice>(item);
        }


        public IDataReader PerformSearch(string whereclause)
        {
            return apinvoceDAL.PerformSearch(whereclause);
        }
    }
}
