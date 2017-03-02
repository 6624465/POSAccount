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
    public class CurrencyBO
    {
        private CurrencyDAL currencyDAL;
        public CurrencyBO()
        {

            currencyDAL = new CurrencyDAL();
        }

        public List<Currency> GetList()
        {
            return currencyDAL.GetList();
        }


        public bool SaveCurrency(Currency newItem)
        {

            return currencyDAL.Save(newItem);

        }

        public bool DeleteCurrency(Currency item)
        {
            return currencyDAL.Delete(item);
        }

        public Currency GetCurrency(Currency item)
        {
            return (Currency)currencyDAL.GetItem<Currency>(item);
        }

    }
}
