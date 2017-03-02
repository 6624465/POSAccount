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
    

    public class GLJournalBO
    {
        private GLJournalDAL GLJournalDAL;
        public GLJournalBO()
        {

            GLJournalDAL = new GLJournalDAL();
        }

        public List<GLJournal> GetList(short branchID)
        {
            return GLJournalDAL.GetList(branchID);
        }


        public bool SaveGLJournal(GLJournal newItem)
        {

            return GLJournalDAL.Save(newItem);

        }


        public bool DeleteGLJournal(string documentNo, short branchID)
        {
            return GLJournalDAL.Delete(documentNo,branchID);
        }




        public bool DeleteGLJournal(GLJournal item)
        {
            return GLJournalDAL.Delete(item);
        }

        public GLJournal GetGLJournal(GLJournal item)
        {
            return (GLJournal)GLJournalDAL.GetItem<GLJournal>(item);
        }

        public IDataReader PerformSearch(string whereclause)
        {
            return GLJournalDAL.PerformSearch(whereclause);
        }
    }

}
