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
    public class APDebitNoteDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APDebitNoteDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APDebitNoteDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPDEBITNOTEDETAIL, MapBuilder<APDebitNoteDetail>.BuildAllProperties()).ToList();
        }

        public List<APDebitNoteDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPDEBITNOTEDETAIL, MapBuilder<APDebitNoteDetail>.BuildAllProperties(), documentNo).ToList();
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

            var apdebitnotedetail = (APDebitNoteDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPDEBITNOTEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apdebitnotedetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, apdebitnotedetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, apdebitnotedetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, apdebitnotedetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo", System.Data.DbType.String, apdebitnotedetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apdebitnotedetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apdebitnotedetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apdebitnotedetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apdebitnotedetail.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apdebitnotedetail.TaxAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax", System.Data.DbType.Decimal, apdebitnotedetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode", System.Data.DbType.String, apdebitnotedetail.TaxCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apdebitnotedetail.Remark);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apdebitnotedetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apdebitnotedetail.ModifiedBy);




                result = db.ExecuteNonQuery(savecommand, transaction);

                if (currentTransaction == null)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw ex;
            }

            return (result > 0 ? true : false);

        }


        public bool Delete(string documentNo, DbTransaction parentTransaction)
        {
            var item = new APDebitNoteDetail { DocumentNo = documentNo };

            return Delete(item, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }




        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var apdebitnotedetail = (APDebitNoteDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPDEBITNOTEDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apdebitnotedetail.DocumentNo);
                

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
            var item = ((APDebitNoteDetail)lookupItem);

            var apdebitnotedetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPDEBITNOTEDETAIL,
                                                    MapBuilder<APDebitNoteDetail>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return apdebitnotedetailItem;
        }

        #endregion






    }
}

