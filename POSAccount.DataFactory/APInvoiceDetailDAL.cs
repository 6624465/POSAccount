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
    public class APInvoiceDetailDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public APInvoiceDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APInvoiceDetail> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPINVOICEDETAIL, MapBuilder<APInvoiceDetail>.BuildAllProperties()).ToList();
        }

        public List<APInvoiceDetail> GetListByDocumentNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPINVOICEDETAIL, MapBuilder<APInvoiceDetail>.BuildAllProperties(), documentNo).ToList();
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

            var apinvoicedetail = (APInvoiceDetail)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPINVOICEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, apinvoicedetail.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, apinvoicedetail.ItemNo);
                db.AddInParameter(savecommand, "AccountCode", System.Data.DbType.String, apinvoicedetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, apinvoicedetail.ChargeCode==null?"":apinvoicedetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo", System.Data.DbType.String, apinvoicedetail.OrderNo==null? "": apinvoicedetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, apinvoicedetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, apinvoicedetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount", System.Data.DbType.Decimal, apinvoicedetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount", System.Data.DbType.Decimal, apinvoicedetail.LocalAmount);
                db.AddInParameter(savecommand, "DiscountType", System.Data.DbType.String, apinvoicedetail.DiscountType);
                db.AddInParameter(savecommand, "Discount", System.Data.DbType.Decimal, apinvoicedetail.Discount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, apinvoicedetail.TotalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, apinvoicedetail.TaxAmount);
                db.AddInParameter(savecommand, "WHAmount", System.Data.DbType.Decimal, apinvoicedetail.WHAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax", System.Data.DbType.Decimal, apinvoicedetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode", System.Data.DbType.String, apinvoicedetail.TaxCode==null?"":apinvoicedetail.TaxCode);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, apinvoicedetail.Remark==null?"":apinvoicedetail.Remark);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, apinvoicedetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, apinvoicedetail.ModifiedBy);





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
            var apInvoicedetailItem = new APInvoiceDetail { DocumentNo = documentNo };

            return Delete(apInvoicedetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var apinvoicedetail = (APInvoiceDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPINVOICEDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apinvoicedetail.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.Int16, apinvoicedetail.ItemNo);

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
            var item = ((APInvoiceDetail)lookupItem);

            var apinvoicedetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPINVOICEDETAIL,
                                                    MapBuilder<APInvoiceDetail>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return apinvoicedetailItem;
        }

        #endregion






    }
}

