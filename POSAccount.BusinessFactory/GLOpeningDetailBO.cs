using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class GLOpeningDetailBO
    {
        private GLOpeningDetailDAL   glopeningdetailDAL;
        public GLOpeningDetailBO() {

            glopeningdetailDAL = new GLOpeningDetailDAL();
        }

        public List<GLOpeningDetail> GetList(string documentNo)
        {
            return glopeningdetailDAL.GetList(documentNo);
        }


        public bool SaveGLOpeningDetail(GLOpeningDetail newItem)
        {

            return glopeningdetailDAL.Save(newItem);

        }

        public bool DeleteGLOpeningDetail(GLOpeningDetail item)
        {
            return glopeningdetailDAL.Delete(item);
        }

        public GLOpeningDetail GetGLOpeningDetail(GLOpeningDetail item)
        {
            return (GLOpeningDetail)glopeningdetailDAL.GetItem<GLOpeningDetail>(item);
        }

    }
}
