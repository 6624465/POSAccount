﻿using System;
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
    public class QuotationItemDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuotationItemDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<QuotationItem> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTQUOTATIONITEM, MapBuilder<QuotationItem>.BuildAllProperties()).ToList();
        }

        public List<QuotationItem> GetListByQuotationNo(string quotationNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTQUOTATIONITEM, MapBuilder<QuotationItem>.BuildAllProperties(), quotationNo).ToList();
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


        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }





        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var quotationitem = (QuotationItem)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEQUOTATIONITEM);

                db.AddInParameter(savecommand, "QuotationNo", System.Data.DbType.String, quotationitem.QuotationNo);
                db.AddInParameter(savecommand, "ItemID", System.Data.DbType.Int16, quotationitem.ItemID);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, quotationitem.ChargeCode);
                db.AddInParameter(savecommand, "SellRate", System.Data.DbType.Decimal, quotationitem.SellRate);
                db.AddInParameter(savecommand, "SlabRate", System.Data.DbType.Boolean, quotationitem.SlabRate);
                db.AddInParameter(savecommand, "SlabRateFrom", System.Data.DbType.Int16, quotationitem.SlabRateFrom);
                db.AddInParameter(savecommand, "SlabRateTo", System.Data.DbType.Int16, quotationitem.SlabRateTo);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, quotationitem.Status);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, quotationitem.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, quotationitem.ModifiedBy);


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


        public bool Delete(string quotationNo, DbTransaction parentTransaction)
        {
            var quotationdetailItem = new QuotationItem { QuotationNo = quotationNo };

            return Delete(quotationdetailItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }





        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var quotationitem = (QuotationItem)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEQUOTATIONITEM);

                db.AddInParameter(deleteCommand, "QuotationNo", System.Data.DbType.String, quotationitem.QuotationNo);


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
            var item = ((QuotationItem)lookupItem);

            var quotationitemItem = db.ExecuteSprocAccessor(DBRoutine.SELECTQUOTATIONITEM,
                                                    MapBuilder<QuotationItem>.BuildAllProperties(),
                                                    item.QuotationNo).FirstOrDefault();
            return quotationitemItem;
        }

        #endregion






    }
}

