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
    public class DebtorDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public DebtorDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<Debtor> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTDEBTOR, MapBuilder<Debtor>.MapAllProperties()
                                                    .DoNotMap(c => c.DebtorAddress)
                                                    .Build()).ToList();
        }

        public List<Debtor> GetListAutoSearch(string searchText)
        {
            return  db.ExecuteSprocAccessor(DBRoutine.LISTDEBTORAUTOSEARCH,
                                                    MapBuilder<Debtor>
                                                    .MapAllProperties()
                                                    .DoNotMap(d=> d.DebtorAddress)
                                                    .Build(),
                                                    searchText).ToList();
        }

        

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var debtor = (Debtor)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVEDEBTOR);

                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, debtor.DebtorCode.ToUpper() == "NEW" ? "" : debtor.DebtorCode);
                db.AddInParameter(savecommand, "DebtorName", System.Data.DbType.String, debtor.DebtorName);
                db.AddInParameter(savecommand, "RegistrationNo", System.Data.DbType.String, debtor.RegistrationNo);
                db.AddInParameter(savecommand, "VATNo", System.Data.DbType.String, debtor.VATNo == null ? "" : debtor.VATNo);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, debtor.CreditTerm);
                db.AddInParameter(savecommand, "DebtorAccount", System.Data.DbType.String, debtor.DebtorAccount);
                db.AddInParameter(savecommand, "PaymentType", System.Data.DbType.String, debtor.PaymentType);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, debtor.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, debtor.ModifiedBy);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, debtor.Status);
                db.AddInParameter(savecommand, "IsAutoSendInvoice", System.Data.DbType.Boolean, debtor.IsAutoSendInvoice);
                db.AddOutParameter(savecommand, "NewDebtorCode", System.Data.DbType.String, 10);


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    debtor.DebtorCode = savecommand.Parameters["@NewDebtorCode"].Value.ToString();

                    AddressDAL addressDAL = new AddressDAL();
                    debtor.DebtorAddress.CreatedBy = debtor.CreatedBy;
                    debtor.DebtorAddress.ModifiedBy = debtor.ModifiedBy;
                    debtor.DebtorAddress.AddressLinkID = debtor.DebtorCode;
                    result = addressDAL.Save(debtor.DebtorAddress, transaction) == true ? 1 : 0;
                }

                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }
            catch (Exception ex)
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
            var debtor = (Debtor)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETEDEBTOR);

                db.AddInParameter(deleteCommand, "DebtorCode", System.Data.DbType.String, debtor.DebtorCode);


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
            var item = ((Debtor)lookupItem);

            var debtorItem = db.ExecuteSprocAccessor(DBRoutine.SELECTDEBTOR,
                                                    MapBuilder<Debtor>
                                                    .MapAllProperties()
                                                    .DoNotMap(d=> d.DebtorAddress)
                                                    .Build(),
                                                    item.DebtorCode).FirstOrDefault();

            if (debtorItem == null)
            {
                return null;
            }

            debtorItem.DebtorAddress = GetDebtorAddress(debtorItem);


            return debtorItem;
        }

        #endregion




        public Address GetDebtorAddress(Debtor debtorItem)
        {
            var contactitem = new POSAccount.Contract.Address
            {
                AddressLinkID = debtorItem.DebtorCode,
                AddressType = "Debtor"
            };

            var currentAddress = new AddressDAL().GetContactsByCustomer(contactitem).FirstOrDefault();


            //companyItem.ContactItem =  new ContactDAL().GetItem(contactItem);

            return currentAddress;



        }


        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHDEBTOR);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }



    }
}

