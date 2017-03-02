using POSAccount.Contract;
using POSAccount.DataFactory;
using System.Collections.Generic;

namespace POSAccount.BusinessFactory
{
    public class APFileUploadBO
    {
        private POSAccount.DataFactory.APFileUploadDAL apfileuploadDAL;
        public APFileUploadBO() {

            apfileuploadDAL = new APFileUploadDAL();
        }

        public List<APFileUpload> GetList(short branchID)
        {
            return apfileuploadDAL.GetList(branchID);
        }


        public bool SaveAPFileUpload(APFileUpload newItem)
        {

            return apfileuploadDAL.Save(newItem);

        }

        public bool DeleteAPFileUpload(APFileUpload item)
        {
            return apfileuploadDAL.Delete(item);
        }

        public APFileUpload GetAPFileUpload(APFileUpload item)
        {
            return (APFileUpload)apfileuploadDAL.GetItem<APFileUpload>(item);
        }

    }
}
