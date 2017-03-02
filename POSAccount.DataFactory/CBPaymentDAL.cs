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
    public class CBPaymentDAL
    {
        private Database db;
        private DbTransaction currentTransaction = null;
        private DbConnection connection = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CBPaymentDAL()
        {

            db = DatabaseFactory.CreateDatabase("POS");

        }

        #region IDataFactory Members

        public List<CBPayment> GetList()
        {
            return db.ExecuteSprocAccessor(DBRoutine.LISTCBPAYMENT, MapBuilder<CBPayment>
                                                                    .MapAllProperties()
                                                                    .DoNotMap(d => d.CBPaymentDetails)
                                                                    .DoNotMap(d => d.CBPaymentSetOffDetails)
                                                                    .DoNotMap(d => d.CBPaymentGLDetails)
                                                                    .Build()
                                                                    ).ToList();
        }

        public bool Save<T>(T item, DbTransaction parentTransaction) where T : IContract
        {
            currentTransaction = parentTransaction;
            return Save(item);

        }




        public bool Save<T>(T item) where T : IContract
        {
            var result = 0;

            var cbpayment = (CBPayment)(object)item;

            if (currentTransaction == null)
            {
                connection = db.CreateConnection();
                connection.Open();
            }

            var transaction = (currentTransaction == null ? connection.BeginTransaction() : currentTransaction);


            try
            {
                var savecommand = db.GetStoredProcCommand(DBRoutine.SAVECBPAYMENT);

                db.AddInParameter(savecommand, "DocumentNo", System.Data.DbType.String, cbpayment.DocumentNo);
                db.AddInParameter(savecommand, "DocumentDate", System.Data.DbType.DateTime, cbpayment.DocumentDate);
                db.AddInParameter(savecommand, "BranchID", System.Data.DbType.Int16, cbpayment.BranchID);
                db.AddInParameter(savecommand, "PaymentType", System.Data.DbType.String, cbpayment.PaymentType ?? "");
                db.AddInParameter(savecommand, "BankCode", System.Data.DbType.String, cbpayment.BankCode);
                db.AddInParameter(savecommand, "CreditorCode", System.Data.DbType.String, cbpayment.CreditorCode ?? "");
                db.AddInParameter(savecommand, "PayeeName", System.Data.DbType.String, cbpayment.PayeeName ?? "");
                db.AddInParameter(savecommand, "BankAccount", System.Data.DbType.String, cbpayment.BankAccount ?? "");
                db.AddInParameter(savecommand, "BankChargesAccount", System.Data.DbType.String, cbpayment.BankChargesAccount ?? "");
                db.AddInParameter(savecommand, "CreditorAccount", System.Data.DbType.String, cbpayment.CreditorAccount ?? "");
                db.AddInParameter(savecommand, "ChequeNo", System.Data.DbType.String, cbpayment.ChequeNo ?? "");
                db.AddInParameter(savecommand, "ChequeDate", System.Data.DbType.DateTime, cbpayment.ChequeDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : cbpayment.ChequeDate);
                db.AddInParameter(savecommand, "CurrencyCode", System.Data.DbType.String, cbpayment.CurrencyCode ?? "THB");
                db.AddInParameter(savecommand, "ExchangeRate", System.Data.DbType.Decimal, cbpayment.ExchangeRate);
                db.AddInParameter(savecommand, "BasePaymentAmount", System.Data.DbType.Decimal, cbpayment.BasePaymentAmount);
                db.AddInParameter(savecommand, "BaseApplyAmount", System.Data.DbType.Decimal, cbpayment.BaseApplyAmount);
                db.AddInParameter(savecommand, "LocalPaymentAmount", System.Data.DbType.Decimal, cbpayment.LocalPaymentAmount);
                db.AddInParameter(savecommand, "LocalApplyAmount", System.Data.DbType.Decimal, cbpayment.LocalApplyAmount);
                db.AddInParameter(savecommand, "LocalDiscountAmount", System.Data.DbType.Decimal, cbpayment.LocalDiscountAmount);
                db.AddInParameter(savecommand, "LocalBankChargesAmount", System.Data.DbType.Decimal, cbpayment.LocalBankChargesAmount);
                db.AddInParameter(savecommand, "Remark", System.Data.DbType.String, cbpayment.Remark ?? "");
                db.AddInParameter(savecommand, "AccountDate", System.Data.DbType.DateTime, cbpayment.AccountDate.ToString("dd-MM-yyyy") == "01-01-0001" ? (object)DBNull.Value : cbpayment.AccountDate);
                db.AddInParameter(savecommand, "Source", System.Data.DbType.String, cbpayment.Source);

                db.AddInParameter(savecommand, "IsWHTax", System.Data.DbType.Boolean, cbpayment.IsWHTax);
                db.AddInParameter(savecommand, "WHPercent", System.Data.DbType.Int16, cbpayment.WHPercent);
                db.AddInParameter(savecommand, "WHAmount", System.Data.DbType.Decimal, cbpayment.WHAmount);
                db.AddInParameter(savecommand, "IsVAT", System.Data.DbType.Boolean, cbpayment.IsVAT);
                db.AddInParameter(savecommand, "TaxAmount", System.Data.DbType.Decimal, cbpayment.TaxAmount);



                db.AddInParameter(savecommand, "NetAmount", System.Data.DbType.Decimal, cbpayment.NetAmount);


                db.AddInParameter(savecommand, "CreatedBy", System.Data.DbType.String, cbpayment.CreatedBy);
                db.AddInParameter(savecommand, "ModifiedBy", System.Data.DbType.String, cbpayment.ModifiedBy);
                db.AddOutParameter(savecommand, "NewDocumentNo", System.Data.DbType.String, 25);


                result = db.ExecuteNonQuery(savecommand, transaction);

                if (result > 0)
                {
                    var cbPaymentDetailsItemDAL = new CBPaymentDetailDAL();
                    var cbPaymentDetailsSetOffItemDAL = new CBPaymentSetOffDetailDAL();
                    var cbpaymentgldetailsItemDAL = new CBPaymentGlDetailDAL();
                    var cbPaymentgltransactionItemDAL = new GLTransactionDAL();

                    short detailitem = 1;


                    // Get the New Quotation No.
                    cbpayment.DocumentNo = savecommand.Parameters["@NewDocumentNo"].Value.ToString();

                    /* Save CBPaymentSetOffDetails if the CBPayment Type ='CREDITOR' */

                    
                    if (cbpayment.CBPaymentSetOffDetails != null)
                    {
                        result = Convert.ToInt16(cbPaymentDetailsSetOffItemDAL.Delete(cbpayment.DocumentNo, transaction));

                        cbpayment.CBPaymentSetOffDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = cbpayment.DocumentNo;
                            dt.CreatedBy = cbpayment.CreatedBy;
                            dt.ModifiedBy = cbpayment.ModifiedBy;
                            dt.CurrencyCode = cbpayment.CurrencyCode;
                            dt.MatchDocumentDate = cbpayment.DocumentDate;
                            dt.CreditorCode = cbpayment.CreditorCode;
                            dt.SetOffDate = cbpayment.DocumentDate;
                        });

                        
                        result = cbPaymentDetailsSetOffItemDAL.SaveList(cbpayment.CBPaymentSetOffDetails, transaction) == true ? 1 : 0;
                    }



                    /* Save CBPaymentGLDetails if the CBPayment Type ='CREDITOR' */

                    
                    if (cbpayment.CBPaymentGLDetails != null)
                    {
                        result = Convert.ToInt16(cbpaymentgldetailsItemDAL.Delete(cbpayment.DocumentNo, transaction));

                        cbpayment.CBPaymentGLDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = cbpayment.DocumentNo;
                        });

                        
                        result = cbpaymentgldetailsItemDAL.SaveList(cbpayment.CBPaymentGLDetails, transaction) == true ? 1 : 0;
                    }

                    if (cbpayment.PaymentType == "CREDITOR")
                    {

                        cbpayment.GLTransactionDetails = new List<GLTransaction>();


                        cbpayment.CBPaymentGLDetails.ForEach(gl =>
                        {
                            var gltransaction = new GLTransaction();

                            gltransaction.AccountCode = gl.AccountCode;
                            gltransaction.AccountDate = cbpayment.DocumentDate;
                            gltransaction.BankInSlipNo = "";
                            gltransaction.BankStatementPgNo = 0;
                            gltransaction.BankStatementTotalPgNo = 0;
                            gltransaction.BaseAmount = gl.TotalAmount;
                            gltransaction.BranchID = cbpayment.BranchID;
                            gltransaction.ChequeNo = "";
                            gltransaction.CreditAmount = 0;
                            gltransaction.CreditorCode = cbpayment.CreditorCode;
                            gltransaction.CurrencyCode = cbpayment.CurrencyCode;
                            gltransaction.DebitAmount = 0;
                            gltransaction.DebitCredit = "";
                            gltransaction.DebtorCode = "";
                            gltransaction.DetailRemark = cbpayment.Remark;
                            gltransaction.DocumentType = "PAY";
                            gltransaction.DocumentNo = cbpayment.DocumentNo;
                            gltransaction.DocumentDate = cbpayment.DocumentDate;
                            gltransaction.Amount = gl.TotalAmount;
                            gltransaction.Remark = cbpayment.Remark;
                            gltransaction.ItemNo = Convert.ToInt16(gl.ItemNo);
                            gltransaction.Source = "CB";
                            gltransaction.AccountDate = cbpayment.DocumentDate;
                            gltransaction.ExchangeRate = 0;
                            gltransaction.Status = true;
                            gltransaction.Year = 0;



                            cbpayment.GLTransactionDetails.Add(gltransaction);
                        });

                    }


                    

                    if (cbpayment.CBPaymentDetails != null)
                    {
                        result = Convert.ToInt16(cbPaymentDetailsItemDAL.Delete(cbpayment.DocumentNo, transaction));

                        detailitem = 1;

                        cbpayment.CBPaymentDetails.ForEach(dt =>
                        {
                            dt.DocumentNo = cbpayment.DocumentNo;
                            dt.ItemNo = detailitem++;
                        });

                        result = cbPaymentDetailsItemDAL.SaveList(cbpayment.CBPaymentDetails, transaction) == true ? 1 : 0;
                    }


                    if (cbpayment.GLTransactionDetails != null)
                    {
                        detailitem = 1;
                        if (cbpayment.PaymentType != "CREDITOR")
                        {
                            cbpayment.GLTransactionDetails.ForEach(dt =>
                            {
                                dt.DocumentNo = cbpayment.DocumentNo;
                                dt.CurrencyCode = cbpayment.CurrencyCode;
                                dt.CreditorCode = cbpayment.CreditorCode;
                                dt.DocumentDate = cbpayment.DocumentDate;
                                dt.BankInSlipNo = "";
                                dt.Amount = dt.CreditAmount > 0 ? dt.CreditAmount : dt.DebitAmount;
                                dt.BaseAmount = dt.CreditAmount > 0 ? dt.CreditAmount : dt.DebitAmount;
                                dt.Remark = dt.Remark?? dt.DetailRemark;
                                dt.ItemNo = detailitem++;
                                dt.Source = "CB";
                                dt.BranchID = cbpayment.BranchID;
                                dt.AccountDate = cbpayment.DocumentDate;
                                dt.DocumentType = "PAY";


                            });
                        }

                        //result = Convert.ToInt16(cbPaymentgltransactionItemDAL.Delete(cbpayment.DocumentNo, transaction));
                        result = cbPaymentgltransactionItemDAL.SaveList(cbpayment.GLTransactionDetails, transaction) == true ? 1 : 0;
                    }


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
            var cbpayment = (CBPayment)(object)item;

            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBPAYMENT);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, cbpayment.DocumentNo);


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


        public bool Delete(string documentNo, string cancelledBy)
        {
            var result = false;


            var connnection = db.CreateConnection();
            connnection.Open();

            var transaction = connnection.BeginTransaction();

            try
            {
                var deleteCommand = db.GetStoredProcCommand(DBRoutine.DELETECBPAYMENT);

                db.AddInParameter(deleteCommand, "DocumentNo", System.Data.DbType.String, documentNo);
                db.AddInParameter(deleteCommand, "CancelledBy", System.Data.DbType.String, cancelledBy);


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
            var item = ((CBPayment)lookupItem);

            var cbpaymentItem = db.ExecuteSprocAccessor(DBRoutine.SELECTCBPAYMENT,
                                                    MapBuilder<CBPayment>
                                                           .MapAllProperties()
                                                           .DoNotMap(d => d.CBPaymentDetails)
                                                            .DoNotMap(d => d.CBPaymentSetOffDetails)
                                                            .DoNotMap(d => d.CBPaymentGLDetails)
                                                           .Build(),
                                                    item.DocumentNo).FirstOrDefault();


            if (cbpaymentItem == null)
                return null;


            if (cbpaymentItem != null)
            {
                cbpaymentItem.CBPaymentDetails = new POSAccount.DataFactory.CBPaymentDetailDAL().GetListByDocumentNo(cbpaymentItem.DocumentNo);
            }


            return cbpaymentItem;
        }

        #endregion



        public System.Data.IDataReader PerformSearch(string whereClause)
        {
            var searchcommand = db.GetStoredProcCommand(DBRoutine.SEARCHPAYMENT);
            db.AddInParameter(searchcommand, "whereClause", System.Data.DbType.String, whereClause);
            return db.ExecuteReader(searchcommand);
        }





    }
}

