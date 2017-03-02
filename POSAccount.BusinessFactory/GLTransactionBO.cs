using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class GLTransactionBO
    {
        private GLTransactionDAL   gltransactionDAL;
        public GLTransactionBO() {

            gltransactionDAL = new GLTransactionDAL();
        }

        public List<GLTransaction> GetList(string documentNo)
        {
            return gltransactionDAL.GetList(documentNo);
        }


        public bool SaveGLTransaction(GLTransaction newItem)
        {

            return gltransactionDAL.Save(newItem);

        }

        public bool DeleteGLTransaction(GLTransaction item)
        {
            return gltransactionDAL.Delete(item);
        }

        public GLTransaction GetGLTransaction(GLTransaction item)
        {
            return (GLTransaction)gltransactionDAL.GetItem<GLTransaction>(item);
        }

    }
}
