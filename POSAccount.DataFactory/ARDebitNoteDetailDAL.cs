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
    public class ARDebitNoteDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARDebitNoteDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARDebitNoteDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARDEBITNOTEDETAIL, MapBuilder<ARDebitNoteDetail>.BuildAllProperties()).ToList();
        }

        public List<ARDebitNoteDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARDEBITNOTEDETAIL, MapBuilder<ARDebitNoteDetail>.BuildAllProperties(), documentNo).ToList();
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

            var ardebitnotedetail = (ARDebitNoteDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARDEBITNOTEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, ardebitnotedetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, ardebitnotedetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, ardebitnotedetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, ardebitnotedetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo", System.Data.DbType.String, ardebitnotedetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, ardebitnotedetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, ardebitnotedetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, ardebitnotedetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, ardebitnotedetail.LocalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, ardebitnotedetail.TaxAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax", System.Data.DbType.Decimal, ardebitnotedetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode", System.Data.DbType.String, ardebitnotedetail.TaxCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, ardebitnotedetail.Remark);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, ardebitnotedetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, ardebitnotedetail.ModifiedBy);



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
            var arDebitNotedetailItem = new ARDebitNoteDetail { DocumentNo = documentNo };

            return Delete(arDebitNotedetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }


        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var ardebitnotedetail = (ARDebitNoteDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARDEBITNOTEDETAIL);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, ardebitnotedetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, ardebitnotedetail.ItemNo);

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
            var item = ((ARDebitNoteDetail)lookupItem);

            var ardebitnotedetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARDEBITNOTEDETAIL,
                                                    MapBuilder<ARDebitNoteDetail>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return ardebitnotedetailItem;
        }

        #endregion






    }
}

