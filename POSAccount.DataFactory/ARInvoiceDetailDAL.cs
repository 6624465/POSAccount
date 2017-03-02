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
    public class ARInvoiceDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARInvoiceDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<ARInvoiceDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARINVOICEDETAIL, MapBuilder<ARInvoiceDetail>.BuildAllProperties()).ToList();
        }

        public List<ARInvoiceDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTARINVOICEDETAIL, MapBuilder<ARInvoiceDetail>.BuildAllProperties(), documentNo).ToList();
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

            var arinvoicedetail = (ARInvoiceDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEARINVOICEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, arinvoicedetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, arinvoicedetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, arinvoicedetail.AccountCode == null ? "" : arinvoicedetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, arinvoicedetail.ChargeCode == null ? "" : arinvoicedetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo", System.Data.DbType.String, arinvoicedetail.OrderNo == null ? "" : arinvoicedetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, arinvoicedetail.CurrencyCode == null ? "THB" : arinvoicedetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, arinvoicedetail.ExchangeRate == null ? 1 : arinvoicedetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, arinvoicedetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, arinvoicedetail.LocalAmount);
                db.AddInParameter(savecommand, "DiscountType", System.Data.DbType.String, arinvoicedetail.DiscountType);
                db.AddInParameter(savecommand, "Discount", System.Data.DbType.Decimal, arinvoicedetail.Discount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, arinvoicedetail.TotalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, arinvoicedetail.TaxAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax", System.Data.DbType.Decimal, arinvoicedetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode", System.Data.DbType.String, arinvoicedetail.TaxCode == null ? "" : arinvoicedetail.TaxCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, arinvoicedetail.Remark ?? "");
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, arinvoicedetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, arinvoicedetail.ModifiedBy);


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
            var arInvoicedetailItem = new ARInvoiceDetail { DocumentNo = documentNo };

            return Delete(arInvoicedetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }



        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var arinvoicedetail = (ARInvoiceDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEARINVOICEDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, arinvoicedetail.DocumentNo);


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
            var item = ((ARInvoiceDetail)lookupItem);

            var arinvoicedetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTARINVOICEDETAIL,
                                                    MapBuilder<ARInvoiceDetail>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return arinvoicedetailItem;
        }

        #endregion






    }
}

