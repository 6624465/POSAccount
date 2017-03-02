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
    public class OrderDetailsDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetailsDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<OrderDetails> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTORDERDETAILS, MapBuilder<OrderDetails>.BuildAllProperties()).ToList();
        }

        public List<OrderDetails> GetListByQuotationNo(string documentNo)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTORDERDETAILS, MapBuilder<OrderDetails>.BuildAllProperties(), documentNo).ToList();
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

            var orderdetails = (OrderDetails)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEORDERDETAILS);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, orderdetails.DocumentNo);
                db.AddInParameter(savecommand, "ItemNo", System.Data.DbType.Int16, orderdetails.ItemNo);
                db.AddInParameter(savecommand, "ChargeCode", System.Data.DbType.String, orderdetails.ChargeCode);
                db.AddInParameter(savecommand, "CandidateName", System.Data.DbType.String, orderdetails.CandidateName??"");
                db.AddInParameter(savecommand, "ContactNo", System.Data.DbType.String, orderdetails.ContactNo??"");
                db.AddInParameter(savecommand, "Position", System.Data.DbType.String, orderdetails.Position??"");
                db.AddInParameter(savecommand, "JoiningDate", System.Data.DbType.DateTime, orderdetails.JoiningDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : orderdetails.JoiningDate);
                db.AddInParameter(savecommand, "EmailID", System.Data.DbType.String, orderdetails.EmailID??"");
                db.AddInParameter(savecommand, "JobStatus", System.Data.DbType.Int16, orderdetails.JobStatus);
                db.AddInParameter(savecommand, "Salary", System.Data.DbType.Decimal, orderdetails.Salary);
                db.AddInParameter(savecommand, "SellRate", System.Data.DbType.Decimal, orderdetails.SellRate);
                db.AddInParameter(savecommand, "Quantity", System.Data.DbType.Int64, orderdetails.Quantity);
                db.AddInParameter(savecommand, "SellPrice", System.Data.DbType.Decimal, orderdetails.SellPrice);
                db.AddInParameter(savecommand, "DiscountType", System.Data.DbType.String, orderdetails.DiscountType??"");
                db.AddInParameter(savecommand, "Discount", System.Data.DbType.Decimal, orderdetails.Discount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, orderdetails.TotalAmount);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, orderdetails.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmountWithTax", System.Data.DbType.Decimal, orderdetails.TotalAmountWithTax);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, orderdetails.Status);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, orderdetails.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, orderdetails.ModifiedBy);



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
            var orderDetailsItem = new OrderDetails { DocumentNo = documentNo };

            return Delete(orderDetailsItem, parentTransaction);
        }

        public bool Delete<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Delete(item);
        }



        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var orderdetails = (OrderDetails)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEORDERDETAILS);


                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, orderdetails.DocumentNo);
                db.AddInParameter(deleteCommand, "ItemNo", System.Data.DbType.String, orderdetails.ItemNo);


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
            var item = ((OrderDetails)lookupItem);

            var orderdetailsItem = db.ExecuteSprocAccessor(DBRoutine.SELECTORDERDETAILS,
                                                    MapBuilder<OrderDetails>.BuildAllProperties(),
                                                    item.DocumentNo).FirstOrDefault();
            return orderdetailsItem;
        }


        public OrderDetails GetChargeCodePrice(string customerCode, string chargeCode)  
        {


            var orderdetailsItem = db.ExecuteSprocAccessor(DBRoutine.GETCHARGECODEPRICE,
                                                    MapBuilder<OrderDetails>.BuildAllProperties(),
                                                    customerCode,chargeCode).FirstOrDefault();
            return orderdetailsItem;
        }

        #endregion






    }
}

