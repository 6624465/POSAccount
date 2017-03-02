using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace NetStock.BusinessFactory
{
    public class GLFinancialPeriodBO
    {
        private GLFinancialPeriodDAL   glfinancialperiodDAL;
        public GLFinancialPeriodBO() {

            glfinancialperiodDAL = new GLFinancialPeriodDAL();
        }

        public List<GLFinancialPeriod> GetList(short branchID, int year)
        {
            return glfinancialperiodDAL.GetList(branchID,year);
        }


        public bool SaveGLFinancialPeriod(GLFinancialPeriod newItem)
        {

            return glfinancialperiodDAL.Save(newItem);

        }

        public bool SaveGLFinancialPeriod(List<GLFinancialPeriod> lstFinancialPeriod)
        {
            return glfinancialperiodDAL.SaveList(lstFinancialPeriod);
        }

        public bool DeleteGLFinancialPeriod(GLFinancialPeriod item)
        {
            return glfinancialperiodDAL.Delete(item);
        }

        public GLFinancialPeriod GetGLFinancialPeriod(GLFinancialPeriod item)
        {
            return (GLFinancialPeriod)glfinancialperiodDAL.GetItem<GLFinancialPeriod>(item);
        }

    }
}
