using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using POSAccount.Contract;
using System.Data.Common;

namespace POSAccount.DataFactory
{
     
    public class GLJournalDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLJournalDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLJournalDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLJOURNALDETAIL, MapBuilder<GLJournalDetail>.BuildAllProperties()).ToList();
        }

        public List<GLJournalDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLJOURNALDETAIL, MapBuilder<GLJournalDetail>.BuildAllProperties(), documentNo).ToList();
        }


        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }

        public bool SaveList<T>(List<T> items, DbTransaction parentTransaction) where T : IContract
        {
            var result = true;

            if (items.Count == 0)
                result = true;

            foreach (var item in items)
            {
                result = Save(item, parentTransaction);
                if (result == false) break;
            }


            return result;

        }








        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var gljournaldetail = (GLJournalDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLJOURNALDETAIL);
                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, gljournaldetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, gljournaldetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, gljournaldetail.AccountCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, gljournaldetail.Remark);
                db.AddInParameter(savecommand, "BaseDebitAmount", System.Data.DbType.Decimal, gljournaldetail.BaseDebitAmount);
                db.AddInParameter(savecommand, "BaseCreditAmount", System.Data.DbType.Decimal, gljournaldetail.BaseCreditAmount);
                 





                result = db.ExecuteNonQuery(savecommand, transaction);

                if (currentTransaction == null)
                    transaction.Commit();

            }
            catch (Exception)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete(string documentNo, DbTransaction parentTransaction)
        {
            var GLJournaldetailItem = new GLJournalDetail { DocumentNo = documentNo };

            return Delete(GLJournaldetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var GLJournaldetail = (GLJournalDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLJOURNALDETAIL);
                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, GLJournaldetail.DocumentNo);


                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                if (currentTransaction==null)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw ex;
            }

            return result;
        }

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {
            var item = ((GLJournalDetail)lookupItem);

            var GLJournaldetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTGLJOURNALDETAIL,
                                                    MapBuilder<GLJournalDetail>.BuildAllProperties(),
                                                    item.DocumentNo, item.ItemNo).FirstOrDefault();
            return GLJournaldetailItem;
        }

        #endregion






    }
}
