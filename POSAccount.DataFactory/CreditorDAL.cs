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
    public class CreditorDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreditorDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<Creditor> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCREDITOR,
                                            MapBuilder<Creditor>.MapAllProperties()
                                                    .DoNotMap(c => c.CreditorAddress)
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

            var creditor = (Creditor)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECREDITOR);


                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, creditor.CreditorCode);
                db.AddInParameter(savecommand, "CreditorName", System.Data.DbType.String, creditor.CreditorName);
                db.AddInParameter(savecommand, "RegistrationNo", System.Data.DbType.String, creditor.RegistrationNo);
                db.AddInParameter(savecommand, "VATNo", System.Data.DbType.String, creditor.VATNo==null?"":creditor.VATNo);
                db.AddInParameter(savecommand, "CreditTerm", System.Data.DbType.String, creditor.CreditTerm);
                db.AddInParameter(savecommand, "CreditorAccount", System.Data.DbType.String, creditor.CreditorAccount);
                db.AddInParameter(savecommand, "PaymentType", System.Data.DbType.String, creditor.PaymentType);
                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, creditor.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, creditor.ModifiedBy);
                db.AddInParameter(savecommand, "Status", System.Data.DbType.Boolean, creditor.Status);
                db.AddInParameter(savecommand, "IsAutoSendInvoice", System.Data.DbType.Boolean, creditor.IsAutoSendInvoice);
                


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    AddressDAL addressDAL = new AddressDAL();
                    creditor.CreditorAddress.CreatedBy = creditor.CreatedBy;
                    creditor.CreditorAddress.ModifiedBy = creditor.ModifiedBy;
                    creditor.CreditorAddress.AddressLinkID = creditor.CreditorCode;
                    result = addressDAL.Save(creditor.CreditorAddress , transaction) == true ? 1 : 0;
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
            var creditor = (Creditor)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECREDITOR);

                db.AddInParameter(deleteCommand, "CreditorCode", System.Data.DbType.String, creditor.CreditorCode);



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
            var item = ((Creditor)lookupItem);

            var creditorItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCREDITOR,
                                                    MapBuilder<Creditor>
                                                    .MapAllProperties()
                                                    .DoNotMap(cr=>cr.CreditorAddress)
                                                    .Build(),
                                                    item.CreditorCode).FirstOrDefault();

            if (creditorItem == null)
            {
                return null;
            }

            creditorItem.CreditorAddress = GetCreditorAddress(creditorItem);

            return creditorItem;
        }

        #endregion




        public Address GetCreditorAddress(Creditor creditorItem)
        {
            var contactitem = new POSAccount.Contract.Address
            {
                AddressLinkID = creditorItem.CreditorCode,
                AddressType = "Creditor"
            };

            var currentAddress = new AddressDAL().GetContactsByCustomer(contactitem).FirstOrDefault();


            //companyItem.ContactItem =  new ContactDAL().GetItem(contactItem);

            return currentAddress;



        }


        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHCREDITOR);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }



    }
}

