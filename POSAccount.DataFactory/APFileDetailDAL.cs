using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class APFileDetailDAL  
    {
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public APFileDetailDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<APFileDetail> GetList(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTAPFILEDETAIL, MapBuilder<APFileDetail>.BuildAllProperties(),documentNo).ToList();
        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var apfiledetail = (APFileDetail)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEAPFILEDETAIL);

                db.AddInParameter(savecommand, "DocumentNo",System.Data.DbType.String,apfiledetail.DocumentNo);
                db.AddInParameter(savecommand, "AccountCode",System.Data.DbType.String,apfiledetail.AccountCode);
                db.AddInParameter(savecommand, "ChargeCode",System.Data.DbType.String,apfiledetail.ChargeCode);
                db.AddInParameter(savecommand, "OrderNo",System.Data.DbType.String,apfiledetail.OrderNo);
                db.AddInParameter(savecommand, "CurrencyCode",System.Data.DbType.String,apfiledetail.CurrencyCode);
                db.AddInParameter(savecommand, "ExchangeRate",System.Data.DbType.Decimal,apfiledetail.ExchangeRate);
                db.AddInParameter(savecommand, "BaseAmount",System.Data.DbType.Decimal,apfiledetail.BaseAmount);
                db.AddInParameter(savecommand, "LocalAmount",System.Data.DbType.Decimal,apfiledetail.LocalAmount);
                db.AddInParameter(savecommand, "DiscountType",System.Data.DbType.String,apfiledetail.DiscountType);
                db.AddInParameter(savecommand, "Discount",System.Data.DbType.Decimal,apfiledetail.Discount);
                db.AddInParameter(savecommand, "TotalAmount",System.Data.DbType.Decimal,apfiledetail.TotalAmount);
                db.AddInParameter(savecommand, "TaxAmount",System.Data.DbType.Decimal,apfiledetail.TaxAmount);
                db.AddInParameter(savecommand, "WHAmount",System.Data.DbType.Decimal,apfiledetail.WHAmount);
                db.AddInParameter(savecommand, "LocalAmountWithTax",System.Data.DbType.Decimal,apfiledetail.LocalAmountWithTax);
                db.AddInParameter(savecommand, "TaxCode",System.Data.DbType.String,apfiledetail.TaxCode);
                db.AddInParameter(savecommand, "Remark",System.Data.DbType.String,apfiledetail.Remark);
                db.AddInParameter(savecommand, "CreatedBy",System.Data.DbType.String,apfiledetail.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy",System.Data.DbType.String,apfiledetail.ModifiedBy);




                result = db.ExecuteNonQuery(savecommand, transaction);

                transaction.Commit();

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var apfiledetail = (APFileDetail)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEAPFILEDETAIL);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, apfiledetail.DocumentNo);


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
            var obj = ((APFileDetail)lookupItem);

            var apfiledetailItem = db.ExecuteSprocAccessor(DBRoutine.SELECTAPFILEDETAIL,
                                                    MapBuilder<APFileDetail>.BuildAllProperties(),
                                                    obj.DocumentNo).FirstOrDefault();
            return apfiledetailItem;
        }

        #endregion

    }
}

