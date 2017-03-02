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
    public class ARCreditNoteDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARCreditNoteDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARCreditNoteDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARCREDITNOTEDETAIL, MapBuilder<ARCreditNoteDetail>.BuildAllProperties()).ToList();
        }

        public List<ARCreditNoteDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARCREDITNOTEDETAIL, MapBuilder<ARCreditNoteDetail>.BuildAllProperties(), documentNo).ToList();
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

            var arcreditnotedetail = (ARCreditNoteDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARCREDITNOTEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, arcreditnotedetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, arcreditnotedetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, arcreditnotedetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, arcreditnotedetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo", System.Data.DbType.String, arcreditnotedetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, arcreditnotedetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, arcreditnotedetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, arcreditnotedetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, arcreditnotedetail.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, arcreditnotedetail.TaxAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax", System.Data.DbType.Decimal, arcreditnotedetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode", System.Data.DbType.String, arcreditnotedetail.TaxCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, arcreditnotedetail.Remark);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, arcreditnotedetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, arcreditnotedetail.ModifiedBy);



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
            var arCreditNotedetailItem = new ARCreditNoteDetail { DocumentNo = documentNo };

            return Delete(arCreditNotedetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }


        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var arcreditnotedetail = (ARCreditNoteDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARCREDITNOTEDETAIL);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, arcreditnotedetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, arcreditnotedetail.ItemNo);

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
            var item = ((ARCreditNoteDetail)lookupItem);

            var arcreditnotedetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARCREDITNOTEDETAIL,
                                                    MapBuilder<ARCreditNoteDetail>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return arcreditnotedetailItem;
        }

        #endregion






    }
}

