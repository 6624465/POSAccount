using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Data.Common;

namespace POSAccount.BusinessFactory
{   
    public class QuotationItemBO
    {
        private QuotationItemDAL quotationitemDAL;
        public QuotationItemBO()
        {

            quotationitemDAL = new QuotationItemDAL();
        }

        public List<QuotationItem> GetList()
        {
            return quotationitemDAL.GetList();
        }


        public bool SaveQuotationItem(QuotationItem newItem)
        {

            return quotationitemDAL.Save(newItem);

        }

        public bool DeleteQuotationItem(QuotationItem item)
        {
            return quotationitemDAL.Delete(item);
        }
        public bool DeleteQuotationItem(string quotationNo, DbTransaction parentTransaction)
        {
            return quotationitemDAL.Delete(quotationNo, parentTransaction);
        }


        public QuotationItem GetQuotationItem(QuotationItem item)
        {
            return (QuotationItem)quotationitemDAL.GetItem<QuotationItem>(item);
        }

    }
}
