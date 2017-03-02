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
    public class CBReceiptDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBReceiptDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBReceipt> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBRECEIPT, MapBuilder<CBReceipt>
                                                    .MapAllProperties()
                                                    .DoNotMap(d => d.CBReceiptDetails)
                                                    .DoNotMap(d => d.CBReceiptSetOffDetails)
                                                    .DoNotMap(d => d.CBReceiptGLDetails)
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

            var cbreceipt = (CBReceipt)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBRECEIPT);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbreceipt.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, cbreceipt.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, cbreceipt.BranchID);
                db.AddInParameter(savecommand, "ReceiptType", System.Data.DbType.String, cbreceipt.ReceiptType);
                db.AddInParameter(savecommand, "BankCode", System.Data.DbType.String, cbreceipt.BankCode);
                db.AddInParameter(savecommand, "DebtorCode", System.Data.DbType.String, cbreceipt.DebtorCode);
                db.AddInParameter(savecommand, "PayerName", System.Data.DbType.String, cbreceipt.PayerName ?? "");
                db.AddInParameter(savecommand, "BankAccount", System.Data.DbType.String, cbreceipt.BankAccount ?? "");
                db.AddInParameter(savecommand, "BankChargesAccount", System.Data.DbType.String, cbreceipt.BankChargesAccount ?? "");
                db.AddInParameter(savecommand, "DebtorAccount", System.Data.DbType.String, cbreceipt.DebtorAccount ?? "");
                db.AddInParameter(savecommand, "ChequeNo", System.Data.DbType.String, cbreceipt.ChequeNo ?? "");
                db.AddInParameter(savecommand, "ChequeDate", System.Data.DbType.DateTime, cbreceipt.ChequeDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : cbreceipt.ChequeDate);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbreceipt.CurrencyCode ?? "THB");
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbreceipt.ExchangeRate);
                db.AddInParameter(savecommand, "BaseReceiptAmount", System.Data.DbType.Decimal, cbreceipt.BaseReceiptAmount);
                db.AddInParameter(savecommand, "BaseApplyAmount", System.Data.DbType.Decimal, cbreceipt.BaseApplyAmount);
                db.AddInParameter(savecommand, "LocalReceiptAmount", System.Data.DbType.Decimal, cbreceipt.LocalReceiptAmount);
                db.AddInParameter(savecommand, "LocalApplyAmount", System.Data.DbType.Decimal, cbreceipt.LocalApplyAmount);
                db.AddInParameter(savecommand, "LocalBankChargesAmount", System.Data.DbType.Decimal, cbreceipt.LocalBankChargesAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, cbreceipt.Remark ?? "");
                db.AddInParameter(savecommand, "AccountDate", System.Data.DbType.DateTime, cbreceipt.AccountDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : cbreceipt.AccountDate);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, cbreceipt.Source ?? "CB");

                db.AddInParameter(savecommand, "IsWHTax", System.Data.DbType.Boolean, cbreceipt.IsWHTax);
                db.AddInParameter(savecommand, "WHPercent", System.Data.DbType.Int16, cbreceipt.WHPercent);
                db.AddInParameter(savecommand, "WHAmount", System.Data.DbType.Decimal, cbreceipt.WHAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, cbreceipt.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, cbreceipt.TaxAmount);
                db.AddInParameter(savecommand, "TotalAmount", System.Data.DbType.Decimal, cbreceipt.TotalAmount);


                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, cbreceipt.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, cbreceipt.ModifiedBy);
                db.AddInParameter(savecommand, "PrintRemarks", System.Data.DbType.String, cbreceipt.PrintRemarks??"");
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var cbReceiptDetailsItemDAL = new CBReceiptDetailDAL();

                    var cbReceiptDetailsSetOffItemDAL = new CBReceiptSetOffDetailDAL();
                    var cbreceiptgldetailsItemDAL = new CBReceiptGlDetailDAL();
                    var cbreceiptgltransactionItemDAL = new GLTransactionDAL();

                    short detailitem = 1;


                    cbreceipt.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();

                    if (cbreceipt.CBReceiptSetOffDetails != null)
                    {

                        result = Convert.ToInt16(cbReceiptDetailsSetOffItemDAL.Delete(cbreceipt.DocumentNo, transaction));
                        cbreceipt.CBReceiptSetOffDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = cbreceipt.DocumentNo;
                            dt.CreatedBy = cbreceipt.CreatedBy;
                            dt.ModifiedBy = cbreceipt.ModifiedBy;
                            dt.CurrencyCode = cbreceipt.CurrencyCode;
                            dt.MatchDocumentDate = cbreceipt.DocumentDate;
                            dt.DebtorCode = cbreceipt.DebtorCode;
                            dt.SetOffDate = cbreceipt.DocumentDate;
                            dt.ItemNo = detailitem++;
                        });


                        result = cbReceiptDetailsSetOffItemDAL.SaveList(cbreceipt.CBReceiptSetOffDetails, transaction) == true ? 1 : 0;
                    }



                    /* Save CBPaymentGLDetails if the CBPayment Type ='CREDITOR' */

                    detailitem = 1;
                    if (cbreceipt.CBReceiptGLDetails != null)
                    {
                        result = Convert.ToInt16(cbreceiptgldetailsItemDAL.Delete(cbreceipt.DocumentNo, transaction));

                        cbreceipt.CBReceiptGLDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = cbreceipt.DocumentNo;
                            dt.TransactionType = "REC";
                            dt.ItemNo = detailitem++;        

                        });


                        result = cbreceiptgldetailsItemDAL.SaveList(cbreceipt.CBReceiptGLDetails, transaction) == true ? 1 : 0;
                    }

                    if (cbreceipt.ReceiptType == "DEBTOR")
                    {

                        cbreceipt.GLTransactionDetails = new List<GLTransaction>();


                        cbreceipt.CBReceiptGLDetails.ForEach(gl =>
                        {
                            var gltransaction = new GLTransaction();

                            gltransaction.AccountCode = gl.AccountCode;
                            gltransaction.AccountDate = cbreceipt.DocumentDate;
                            gltransaction.BankInSlipNo = "";
                            gltransaction.BankStatementPgNo = 0;
                            gltransaction.BankStatementTotalPgNo = 0;
                            gltransaction.BaseAmount = gl.TotalAmount;
                            gltransaction.BranchID = cbreceipt.BranchID;
                            gltransaction.ChequeNo = "";
                            gltransaction.CreditAmount = 0;
                            gltransaction.CreditorCode = "";
                            gltransaction.CurrencyCode = cbreceipt.CurrencyCode;
                            gltransaction.DebitAmount = 0;
                            gltransaction.DebitCredit = "";
                            gltransaction.DebtorCode = cbreceipt.DebtorCode;
                            gltransaction.DetailRemark = cbreceipt.Remark;
                            gltransaction.DocumentType = "REC";
                            gltransaction.DocumentNo = cbreceipt.DocumentNo;
                            gltransaction.DocumentDate = cbreceipt.DocumentDate;
                            gltransaction.Amount = gl.TotalAmount;
                            gltransaction.Remark = cbreceipt.Remark;
                            gltransaction.ItemNo = Convert.ToInt16(gl.ItemNo);
                            gltransaction.Source = "CB";
                            gltransaction.AccountDate = cbreceipt.DocumentDate;
                            gltransaction.ExchangeRate = 0;
                            gltransaction.Status = true;
                            gltransaction.Year = 0;



                            cbreceipt.GLTransactionDetails.Add(gltransaction);
                        });

                    }


                    if (cbreceipt.CBReceiptDetails != null)
                    {
                        result = Convert.ToInt16(cbReceiptDetailsItemDAL.Delete(cbreceipt.DocumentNo, transaction));
                        detailitem = 1;

                        cbreceipt.CBReceiptDetails.ForEach(dt =>
                        {
                            dt.ItemNo = detailitem++;
                            dt.DocumentNo = cbreceipt.DocumentNo;
                        });

                        result = cbReceiptDetailsItemDAL.SaveList(cbreceipt.CBReceiptDetails, transaction) == true ? 1 : 0;
                    }

                    if (cbreceipt.GLTransactionDetails != null)
                    {
                        detailitem = 1;
                        if (cbreceipt.ReceiptType != "DEBTOR")
                        {
                            cbreceipt.GLTransactionDetails.ForEach(dt =>
                            {
                                dt.DocumentNo = cbreceipt.DocumentNo;
                                dt.CurrencyCode = cbreceipt.CurrencyCode;
                                dt.DebtorCode = cbreceipt.DebtorCode;
                                dt.DocumentDate = cbreceipt.DocumentDate;
                                dt.BankInSlipNo = "";
                                dt.Amount = dt.CreditAmount > 0 ? dt.CreditAmount : dt.DebitAmount;
                                dt.BaseAmount = dt.CreditAmount > 0 ? dt.CreditAmount : dt.DebitAmount;
                                dt.Remark = dt.Remark?? dt.DetailRemark;
                                dt.ItemNo = detailitem++;
                                dt.Source = "CB";
                                dt.BranchID = cbreceipt.BranchID;
                                dt.AccountDate = cbreceipt.DocumentDate;
                                dt.DocumentType = "REC";


                            });
                        }

                        //result = Convert.ToInt16(cbPaymentgltransactionItemDAL.Delete(cbpayment.DocumentNo, transaction));
                        result = cbreceiptgltransactionItemDAL.SaveList(cbreceipt.GLTransactionDetails, transaction) == true ? 1 : 0;
                    }


                }





                if (result > 0)
                    transaction.Commit();
                else
                    transaction.Rollback();

            }
            catch (Exception  ex)
            {
                if (currentTransaction == null)
                    transaction.Rollback();

                throw;
            }

            return (result > 0 ? true : false);

        }

        public bool Delete(string documentNo, string cancelledBy)
        {
            var result = false;

            var connnection = db.CreateConnection();
            connnection.Open();
            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBRECEIPT);



                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, documentNo);
                db.AddInParameter(deleteCommand, "CanceledBy", System.Data.DbType.String, cancelledBy);

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



        public bool Delete<T>(T item) where T : IContract
        {
            var result = false;
            var cbreceipt = (CBReceipt)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBRECEIPT);



                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbreceipt.DocumentNo);
                db.AddInParameter(deleteCommand, "CanceledBy", System.Data.DbType.String, cbreceipt.ModifiedBy);

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
            var item = ((CBReceipt)lookupItem);

            var cbreceiptItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBRECEIPT,
                                                    MapBuilder<CBReceipt>
                                                    .MapAllProperties()
                                                    .DoNotMap(d => d.CBReceiptDetails)
                                                    .DoNotMap(d => d.CBReceiptSetOffDetails)
                                                    .DoNotMap(d => d.CBReceiptGLDetails)
                                                    .Build(),
                                                    item.DocumentNo).FirstOrDefault();



            if (cbreceiptItem == null)
                return null;


            if (cbreceiptItem != null)
            {
                cbreceiptItem.CBReceiptDetails = new POSAccount.DataFactory.CBReceiptDetailDAL().GetListByDocumentNo(cbreceiptItem.DocumentNo);
                cbreceiptItem.CBReceiptSetOffDetails = new POSAccount.DataFactory.CBReceiptSetOffDetailDAL().GetListByDocumentNo(cbreceiptItem.DocumentNo);
                cbreceiptItem.CBReceiptGLDetails = new POSAccount.DataFactory.CBReceiptGlDetailDAL().GetListByDocumentNo(cbreceiptItem.DocumentNo);
            }


            return cbreceiptItem;
        }

        #endregion




        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHRECEIPT);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }




    }
}

