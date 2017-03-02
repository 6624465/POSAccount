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
    public class BankDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public BankDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<Bank> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTBANK, MapBuilder<Bank>
                                                    .MapAllProperties()
                                                    .DoNotMap(c => c.BankAddress)
                                                    .Build()).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var bank = (Bank)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEBANK);

                db.AddInParameter(savecommand, "BankCode", System.Data.DbType.String, bank.BankCode);
                db.AddInParameter(savecommand, "Name", System.Data.DbType.String, bank.Name);
                db.AddInParameter(savecommand, "AccountNo", System.Data.DbType.String, bank.AccountNo);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, bank.CurrencyCode ?? "");
                db.AddInParameter(savecommand, "BankAccount", System.Data.DbType.String, bank.BankAccount);
                db.AddInParameter(savecommand, "OverdraftLimit", System.Data.DbType.Decimal, bank.OverdraftLimit);
                db.AddInParameter(savecommand, "CurrentBalance", System.Data.DbType.Decimal, bank.CurrentBalance);
                db.AddInParameter(savecommand, "SwiftCode", System.Data.DbType.String, bank.SwiftCode ?? "");
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, bank.Status);
                db.AddInParameter(savecommand, "AddressId", System.Data.DbType.String, bank.AddressId ?? "");
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, bank.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, bank.ModifiedBy);


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    AddressDAL addressDAL = new AddressDAL();
                    bank.BankAddress.CreatedBy = bank.CreatedBy;
                    bank.BankAddress.ModifiedBy = bank.ModifiedBy;
                    bank.BankAddress.AddressLinkID = bank.BankCode;
                    result = addressDAL.Save(bank.BankAddress, transaction) == true ? 1 : 0;
                }

                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();

            }
            catch (Exception)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var bank = (Bank)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEBANK);

                db.AddInParameter(deleteCommand, "BankCode", System.Data.DbType.String, bank.BankCode);

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
            var item = ((Bank)lookupItem);

            var bankItem = db.ExecuteSprocAccessor(DBRoutine.SELECTBANK,
                                                    MapBuilder<Bank>
                                                    .MapAllProperties()
                                                    .DoNotMap(c => c.BankAddress)
                                                    .Build(),
                                                    item.BankCode).FirstOrDefault();

            if (bankItem == null)
            {
                return null;
            }

            bankItem.BankAddress = GetBankAddress(bankItem);



            return bankItem;
        }

        #endregion



        public Address GetBankAddress(Bank bankItem)
        {
            var contactItem = new POSAccount.Contract.Address
            {
                AddressLinkID = bankItem.BankCode,
                AddressType = "Bank"
            };

            var currentAddress = new AddressDAL().GetContactsByCustomer(contactItem).FirstOrDefault();


            //companyItem.ContactItem =  new ContactDAL().GetItem(contactItem);

            return currentAddress;



        }


    }
}

