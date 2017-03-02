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
    public class LookUpBO
    {
        private LookUpDAL lookupDAL;
        public LookUpBO()
        {

            lookupDAL = new LookUpDAL();
        }

        public List<Lookup> GetList()
        {
            return lookupDAL.GetList();
        }


        public bool SaveLookUp(Lookup newItem)
        {

            return lookupDAL.Save(newItem);

        }

        public bool DeleteLookUp(Lookup item)
        {
            return lookupDAL.Delete(item);
        }

        public Lookup GetLookUp(Lookup item)
        {
            return (Lookup)lookupDAL.GetItem<Lookup>(item);
        }

    }
}
