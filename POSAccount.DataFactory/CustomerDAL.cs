using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using POSAccount.Contract;


namespace POSAccount.DataFactory
{
    public class CustomerDAL
    {
        private Database db;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<Customer> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCUSTOMER, MapBuilder<Customer>.BuildAllProperties()).ToList();
        }

        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var customer = (Customer)(object)item;

            var connection = db.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECUSTOMER);

                db.AddInParameter(savecommand, "CustomerCode", System.Data.DbType.String, customer.CustomerCode);
                db.AddInParameter(savecommand, "CustomerName", System.Data.DbType.String, customer.CustomerName);
                db.AddInParameter(savecommand, "RegistrationNo", System.Data.DbType.String, customer.RegistrationNo);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, customer.CreditorCode);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, customer.DebtorCode);
                db.AddInParameter(savecommand, "RevenueAccount", System.Data.DbType.String, customer.RevenueAccount);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, customer.Status);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, customer.Remark);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.Int16, customer.CreditTerm);
                db.AddInParameter(savecommand, "AddressID", System.Data.DbType.String, customer.AddressID);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, customer.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, customer.ModifiedBy);
                db.AddInParameter(savecommand, "CustomerType", System.Data.DbType.String, customer.CustomerType);


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    AddressDAL addressDAL = new AddressDAL();

                    customer.CustomerAddress.CreatedBy = customer.CreatedBy;
                    customer.CustomerAddress.ModifiedBy = customer.ModifiedBy;
                    result = addressDAL.Save(customer.CustomerAddress, transaction) == true ? 1 : 0;
                }

                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();


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
            var customer = (Customer)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECUSTOMER);

                db.AddInParameter(deleteCommand, "CustomerCode", System.Data.DbType.String, customer.CustomerCode);

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
            var item = ((Customer)lookupItem);

            var customerItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCUSTOMER,
                                                    MapBuilder<Customer>
                                                    .MapAllProperties()
                                                    .DoNotMap(c => c.CustomerAddress)
                                                    .Build(),
                                                    ((Customer)lookupItem).CustomerCode).FirstOrDefault();

            if (customerItem == null)
            {
                return null;
            }
            var contactItem = new POSAccount.Contract.Address
            {
                AddressLinkID = customerItem.CustomerCode,
                AddressType = "Customer"
            };

            customerItem.CustomerAddress = new AddressDAL().GetContactsByCustomer(contactItem).FirstOrDefault();




            return customerItem;
        }

        #endregion






    }
}

