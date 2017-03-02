using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class GLOpeningBO
    {
        private GLOpeningDAL   glopeningDAL;
        public GLOpeningBO() {

            glopeningDAL = new GLOpeningDAL();
        }

        public List<GLOpening> GetList(int financialYear, short branchID)
        {
            return glopeningDAL.GetList(financialYear,  branchID);
        }


        public bool SaveGLOpening(GLOpening newItem)
        {

            return glopeningDAL.Save(newItem);

        }

        public bool DeleteGLOpening(GLOpening item)
        {
            return glopeningDAL.Delete(item);
        }

        public GLOpening GetGLOpening(GLOpening item)
        {
            return (GLOpening)glopeningDAL.GetItem<GLOpening>(item);
        }

    }
}
