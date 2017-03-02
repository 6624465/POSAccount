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
     
    public class GLJournalDetailBO
    {
        private GLJournalDetailDAL  GLJournaldetailDAL;
        public GLJournalDetailBO()
        {

             GLJournaldetailDAL = new  GLJournalDetailDAL();
        }

        public List< GLJournalDetail> GetList()
        {
            return  GLJournaldetailDAL.GetList();
        }

        public List< GLJournalDetail> GetListByDocumentNo(string documentNo)
        {
            return  GLJournaldetailDAL.GetListByDocumentNo(documentNo);
        }


        public bool SaveGLJournalDetail(GLJournalDetail newItem)
        {

            return GLJournaldetailDAL.Save(newItem);

        }

        public bool DeleteGLJournalDetail(GLJournalDetail item)
        {
            return GLJournaldetailDAL.Delete(item);
        }

        public GLJournalDetail GetGLJournalDetail(GLJournalDetail item)
        {
            return (GLJournalDetail)GLJournaldetailDAL.GetItem<GLJournalDetail>(item);
        }

    }

}
