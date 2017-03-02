using Microsoft.Practices.EnterpriseLibrary.Data;
using POSAccount.Contract;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace POSAccount.DataFactory
{
    public class AssetHeaderDAL 
    {
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public AssetHeaderDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<AssetHeader> GetList(short branchID)
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTASSETHEADER, MapBuilder<AssetHeader>.BuildAllProperties(),branchID).ToList();
        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var assetheader = (AssetHeader)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEASSETHEADER);

                db.AddInParameter(savecommand, "AssetCode", System.Data.DbType.String, assetheader.AssetCode);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, assetheader.BranchID);
                db.AddInParameter(savecommand, "Description", System.Data.DbType.String, assetheader.Description);
                db.AddInParameter(savecommand, "BuyingDate", System.Data.DbType.DateTime, assetheader.BuyingDate);
                db.AddInParameter(savecommand, "Price", System.Data.DbType.Decimal, assetheader.Price);
                db.AddInParameter(savecommand, "Rate", System.Data.DbType.Decimal, assetheader.Rate);
                db.AddInParameter(savecommand, "DepreciationType", System.Data.DbType.String, assetheader.DepreciationType);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, assetheader.Status);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, assetheader.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, assetheader.ModifiedBy);



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
            var assetheader = (AssetHeader)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEASSETHEADER);

                
                db.AddInParameter(deleteCommand, "AssetCode", System.Data.DbType.String, assetheader.AssetCode);
                db.AddInParameter(deleteCommand, "BranchID", System.Data.DbType.Int16, assetheader.BranchID);
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

            var assetheaderitem =((AssetHeader)lookupItem);

            var Item = db.ExecuteSprocAccessor(DBRoutine.SELECTASSETHEADER,
                                                    MapBuilder<AssetHeader>.BuildAllProperties(),
                                                    assetheaderitem.AssetCode, assetheaderitem.BranchID).FirstOrDefault();
            return Item;
        }

        #endregion

    }
}

