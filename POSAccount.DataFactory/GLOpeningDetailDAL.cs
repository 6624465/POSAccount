using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class GLOpeningDetailDAL  
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLOpeningDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLOpeningDetail> GetList(string documetNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLOPENINGDETAIL, MapBuilder<GLOpeningDetail>.BuildAllProperties(),documetNo).ToList();
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

            var glopeningdetail = (GLOpeningDetail)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLOPENINGDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, glopeningdetail.DocumentNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, glopeningdetail.AccountCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, glopeningdetail.Remark);
                db.AddInParameter(savecommand, "DebitAmount", System.Data.DbType.Decimal, glopeningdetail.DebitAmount);
                db.AddInParameter(savecommand, "CreditAmount", System.Data.DbType.Decimal, glopeningdetail.CreditAmount);



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
            var GLOpeningdetailItem = new GLOpeningDetail { DocumentNo = documentNo };

            return Delete(GLOpeningdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var glopeningdetail = (GLOpeningDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLOPENINGDETAIL);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, glopeningdetail.DocumentNo);
                result = Convert.ToBoolean(db.ExecuteNonQuery(deleteCommand, transaction));

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }

            return result;
        }

        public IContract GetItem<T>(IContract lookupItem) where T : IContract
        {
            var obj = ((GLOpeningDetail)lookupItem);

            var glopeningdetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTGLOPENINGDETAIL,
                                                    MapBuilder<GLOpeningDetail>.BuildAllProperties(),
                                                    obj.DocumentNo).FirstOrDefault();
            return glopeningdetailItem;
        }

        #endregion

    }
}

