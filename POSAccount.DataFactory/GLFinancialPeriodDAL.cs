using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class GLFinancialPeriodDAL
    {   
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public GLFinancialPeriodDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<GLFinancialPeriod> GetList(short branchID, int year)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTGLFINANCIALPERIOD, MapBuilder<GLFinancialPeriod>.BuildAllProperties(),branchID,year).ToList();
        }


         

        public bool SaveList<T>(List<T> items ) where T : IContract
        {
            var result = true;

            if (items.Count == 0)
                result = true;

            foreach (var item in items)
            {
                result = Save(item);
                if (result == false) break;
            }


            return result;

        }







        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var glfinancialperiod = (GLFinancialPeriod)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEGLFINANCIALPERIOD);
                
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, glfinancialperiod.BranchID);
                db.AddInParameter(savecommand, "FinancialYear", System.Data.DbType.Int32, glfinancialperiod.FinancialYear);
                db.AddInParameter(savecommand, "Period", System.Data.DbType.Int16, glfinancialperiod.Period);
                db.AddInParameter(savecommand, "StartDate", System.Data.DbType.DateTime, glfinancialperiod.StartDate);
                db.AddInParameter(savecommand, "IsARClosed", System.Data.DbType.Boolean, glfinancialperiod.IsARClosed==null?false:glfinancialperiod.IsARClosed);
                db.AddInParameter(savecommand, "ARClosedDate", System.Data.DbType.DateTime, glfinancialperiod.ARClosedDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : glfinancialperiod.ARClosedDate);
                db.AddInParameter(savecommand, "ARClosedBy", System.Data.DbType.String, glfinancialperiod.ARClosedBy??"");
                db.AddInParameter(savecommand, "IsAPClosed", System.Data.DbType.Boolean, glfinancialperiod.IsAPClosed == null ? false : glfinancialperiod.IsAPClosed);
                db.AddInParameter(savecommand, "APClosedDate", System.Data.DbType.DateTime, glfinancialperiod.APClosedDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : glfinancialperiod.APClosedDate);
                db.AddInParameter(savecommand, "APClosedBy", System.Data.DbType.String, glfinancialperiod.APClosedBy ?? "");
                db.AddInParameter(savecommand, "IsGLClosed", System.Data.DbType.Boolean, glfinancialperiod.IsGLClosed == null ? false : glfinancialperiod.IsGLClosed);
                db.AddInParameter(savecommand, "GLClosedDate", System.Data.DbType.DateTime, glfinancialperiod.GLClosedDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : glfinancialperiod.GLClosedDate);
                db.AddInParameter(savecommand, "GLClosedBy", System.Data.DbType.String, glfinancialperiod.GLClosedBy ?? "");
                db.AddInParameter(savecommand, "IsCBClosed", System.Data.DbType.Boolean, glfinancialperiod.IsCBClosed == null ? false : glfinancialperiod.IsCBClosed);
                db.AddInParameter(savecommand, "CBClosedDate", System.Data.DbType.DateTime, glfinancialperiod.CBClosedDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : glfinancialperiod.CBClosedDate);
                db.AddInParameter(savecommand, "CBClosedBy", System.Data.DbType.String, glfinancialperiod.CBClosedBy ?? "");


                



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
            var glfinancialperiod = (GLFinancialPeriod)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEGLFINANCIALPERIOD);


                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, glfinancialperiod.BranchID);
                db.AddInParameter(deleteCommand, "Period", System.Data.DbType.Int16, glfinancialperiod.Period);
                db.AddInParameter(deleteCommand, "FinancialYear", System.Data.DbType.Int32, glfinancialperiod.FinancialYear);

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
            var item = (GLFinancialPeriod)lookupItem;

            var glfinancialperiod = db.ExecuteSprocAccessor(DBRoutine.SELECTGLFINANCIALPERIOD,
                                                    MapBuilder<GLFinancialPeriod>.BuildAllProperties(),
                                                    item.BranchID, item.FinancialYear).FirstOrDefault();
            return glfinancialperiod;
        }

        #endregion

    }
}

